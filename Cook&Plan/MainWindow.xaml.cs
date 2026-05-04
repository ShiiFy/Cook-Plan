using Cook_Plan.Core.Adapter;
using Cook_Plan.Core.Builders;
using Cook_Plan.Core.Composite;
using Cook_Plan.Core.Facade;
using Cook_Plan.Core.Factories;
using Cook_Plan.Core.Flyweight;
using Cook_Plan.Core.Prototypes;
using Cook_Plan.Core.Services;
using Cook_Plan.Core.Temporarily;
using Cook_Plan.Domain.Enums;
using Cook_Plan.Domain.Models;
using Cook_Plan.Themes;
using Cook_Plan.Themes.Dark;
using Cook_Plan.Themes.Light;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text;

namespace Cook_Plan
{
    public partial class MainWindow : Window
    {
        private IThemeFactory _themeFactory = new LightThemeFactory();

        private readonly RecipeService _recipeService;
        private readonly IngredientFlyweightFactory _ingredientFlyweightFactory;
        private readonly RecipeCacheManager _recipeCacheManager;
        private readonly MealPlanningFacade _mealPlanningFacade;
        private readonly RecipeImportService _recipeImportService;

        private List<Recipe> _recipes = new();
        private List<Ingredient> _newIngredients = new();
        private List<RecipeStep> _newSteps = new();
        private List<UiMealPlanItem> _mealPlanItems = new();

        private string _currentFg = "#212121";
        private string _currentBg = "#FFFFFF";

        public MainWindow(
            RecipeService recipeService,
            IngredientFlyweightFactory ingredientFlyweightFactory,
            RecipeCacheManager recipeCacheManager,
            MealPlanningFacade mealPlanningFacade,
            RecipeImportService recipeImportService)
        {
            InitializeComponent();

            _recipeService = recipeService;
            _ingredientFlyweightFactory = ingredientFlyweightFactory;
            _recipeCacheManager = recipeCacheManager;
            _mealPlanningFacade = mealPlanningFacade;
            _recipeImportService = recipeImportService;

            InitializeStaticChoices();
            EnsureDatabaseHasData();
            LoadRecipesFromDatabase();
            ApplyTheme();
        }

        private void InitializeStaticChoices()
        {
            CmbRecipeMealType.ItemsSource = Enum.GetValues(typeof(MealType));
            CmbRecipeCuisine.ItemsSource = Enum.GetValues(typeof(Cuisine));
            CmbRecipeDifficulty.ItemsSource = Enum.GetValues(typeof(Difficulty));

            CmbRecipeMealType.SelectedItem = MealType.Dinner;
            CmbRecipeCuisine.SelectedItem = Cuisine.Other;
            CmbRecipeDifficulty.SelectedItem = Difficulty.Easy;

            CmbDay.ItemsSource = new List<string>
            {
                "Понедельник",
                "Вторник",
                "Среда",
                "Четверг",
                "Пятница",
                "Суббота",
                "Воскресенье"
            };

            CmbMealType.ItemsSource = new List<string>
            {
                "Завтрак",
                "Обед",
                "Ужин",
                "Перекус"
            };

            CmbDay.SelectedIndex = 0;
            CmbMealType.SelectedIndex = 0;
        }

        private void EnsureDatabaseHasData()
        {
            var existingRecipes = _recipeService.GetAll();

            if (existingRecipes.Count > 0)
                return;

            var dir = new RecipeDirector(new RecipeBuilder());

            var pasta = dir.Construct(
                "Паста Карбонара",
                "Классическая итальянская паста с беконом и нежным соусом.",
                MealType.Lunch,
                Cuisine.Italian,
                Difficulty.Medium,
                25,
                2,
                new List<Ingredient>
                {
                    new()
                    {
                        ProductId = 1,
                        Amount = 200,
                        Product = new Product
                        {
                            Id = 1,
                            Name = "Спагетти",
                            Unit = "г",
                            CaloriesPer100 = 350
                        }
                    },
                    new()
                    {
                        ProductId = 2,
                        Amount = 100,
                        Product = new Product
                        {
                            Id = 2,
                            Name = "Бекон",
                            Unit = "г",
                            CaloriesPer100 = 400
                        }
                    }
                },
                new List<RecipeStep>
                {
                    new() { StepNumber = 1, Description = "Отварить спагетти до готовности." },
                    new() { StepNumber = 2, Description = "Обжарить бекон на сковороде." },
                    new() { StepNumber = 3, Description = "Смешать пасту с соусом и беконом." }
                });

            var omelette = dir.Construct(
                "Омлет",
                "Быстрый завтрак из яиц.",
                MealType.Breakfast,
                Cuisine.French,
                Difficulty.Easy,
                10,
                1,
                new List<Ingredient>
                {
                    new()
                    {
                        ProductId = 3,
                        Amount = 3,
                        Product = new Product
                        {
                            Id = 3,
                            Name = "Яйца",
                            Unit = "шт",
                            CaloriesPer100 = 155
                        }
                    }
                },
                new List<RecipeStep>
                {
                    new() { StepNumber = 1, Description = "Взбить яйца с солью." },
                    new() { StepNumber = 2, Description = "Обжарить на сковороде до готовности." }
                });

            _recipeService.Add(pasta);
            _recipeService.Add(omelette);
        }

        private void LoadRecipesFromDatabase()
        {
            _recipes = _recipeService.GetAll();

            RefreshRecipeList();
            RefreshRecipeComboboxes();
        }

        private void RefreshRecipeList()
        {
            RecipeList.Items.Clear();

            foreach (var recipe in _recipes)
            {
                RecipeList.Items.Add($"{recipe.Name}  •  {recipe.CookingTimeMinutes} мин  •  {recipe.Servings} порц.");
            }

            BtnClone.IsEnabled = RecipeList.SelectedIndex >= 0;
            BtnDeleteRecipe.IsEnabled = RecipeList.SelectedIndex >= 0;
        }

        private void RefreshRecipeComboboxes()
        {
            CmbRecipes.Items.Clear();
            CmbMealRecipe.Items.Clear();

            foreach (var recipe in _recipes)
            {
                CmbRecipes.Items.Add(recipe.Name);
                CmbMealRecipe.Items.Add(recipe.Name);
            }

            if (CmbRecipes.Items.Count > 0)
                CmbRecipes.SelectedIndex = 0;

            if (CmbMealRecipe.Items.Count > 0)
                CmbMealRecipe.SelectedIndex = 0;
        }

        private void BtnAddIngr_Click(object sender, RoutedEventArgs e)
        {
            var name = TxtIngrName.Text.Trim();
            var unit = TxtIngrUnit.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите название ингредиента.", "Cook&Plan");
                return;
            }

            if (string.IsNullOrWhiteSpace(unit))
                unit = "г";

            if (!double.TryParse(TxtIngrAmount.Text, out double amount) || amount <= 0)
                amount = 1;

            var product = _ingredientFlyweightFactory.GetProduct(name, unit);

            _newIngredients.Add(new Ingredient
            {
                ProductId = _newIngredients.Count + 1,
                Amount = amount,
                Product = product
            });

            IngrList.Items.Add($"• {product.Name} — {amount} {product.Unit}");

            TxtIngrName.Clear();
            TxtIngrAmount.Text = "100";
            TxtIngrUnit.Text = unit;
        }

        private void BtnAddStep_Click(object sender, RoutedEventArgs e)
        {
            var description = TxtStep.Text.Trim();

            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Введите описание шага.", "Cook&Plan");
                return;
            }

            var stepNumber = _newSteps.Count + 1;

            _newSteps.Add(new RecipeStep
            {
                StepNumber = stepNumber,
                Description = description
            });

            StepList.Items.Add($"{stepNumber}. {description}");

            TxtStep.Clear();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                MessageBox.Show("Введите название рецепта.", "Cook&Plan");
                return;
            }

            if (_newIngredients.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы один ингредиент.", "Cook&Plan");
                return;
            }

            if (_newSteps.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы один шаг приготовления.", "Cook&Plan");
                return;
            }

            int.TryParse(TxtTime.Text, out int time);
            int.TryParse(TxtServings.Text, out int servings);

            var recipe = new RecipeBuilder()
                .SetBasicInfo(TxtName.Text.Trim(), TxtDesc.Text.Trim())
                .SetDetails(
                    (MealType)(CmbRecipeMealType.SelectedItem ?? MealType.Dinner),
                    (Cuisine)(CmbRecipeCuisine.SelectedItem ?? Cuisine.Other),
                    (Difficulty)(CmbRecipeDifficulty.SelectedItem ?? Difficulty.Easy))
                .SetCookingInfo(time > 0 ? time : 30, servings > 0 ? servings : 1)
                .AddIngredients(_newIngredients)
                .AddSteps(_newSteps)
                .Build();

            _recipeService.Add(recipe);

            ClearRecipeForm();
            LoadRecipesFromDatabase();

            MessageBox.Show($"Рецепт «{recipe.Name}» сохранён.", "Cook&Plan");
        }

        private void ClearRecipeForm()
        {
            TxtName.Clear();
            TxtDesc.Clear();
            TxtTime.Text = "30";
            TxtServings.Text = "2";

            CmbRecipeMealType.SelectedItem = MealType.Dinner;
            CmbRecipeCuisine.SelectedItem = Cuisine.Other;
            CmbRecipeDifficulty.SelectedItem = Difficulty.Easy;

            _newIngredients = new();
            _newSteps = new();

            IngrList.Items.Clear();
            StepList.Items.Clear();
        }

        private void RecipeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = GetSelectedRecipe();

            BtnClone.IsEnabled = selected != null;
            BtnDeleteRecipe.IsEnabled = selected != null;

            if (selected == null)
            {
                RecipeDetails.Visibility = Visibility.Collapsed;
                return;
            }

            ShowRecipeDetails(selected);
        }

        private Recipe? GetSelectedRecipe()
        {
            var index = RecipeList.SelectedIndex;

            if (index < 0 || index >= _recipes.Count)
                return null;

            return _recipeService.GetById(_recipes[index].Id);
        }

        private void ShowRecipeDetails(Recipe recipe)
        {
            RecipeDetails.Visibility = Visibility.Visible;

            DetailName.Text = recipe.Name;
            DetailInfo.Text =
                $"{recipe.MealType} • {recipe.Cuisine} • {recipe.Difficulty} • {recipe.CookingTimeMinutes} мин • {recipe.Servings} порц.";

            DetailDescription.Text = string.IsNullOrWhiteSpace(recipe.Description)
                ? "Описание не указано."
                : recipe.Description;

            DetailIngredients.Items.Clear();

            if (recipe.Ingredients.Count == 0)
            {
                DetailIngredients.Items.Add("Ингредиенты не указаны.");
            }
            else
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    DetailIngredients.Items.Add(
                        $"• {ingredient.Product?.Name ?? "Продукт"} — {ingredient.Amount} {ingredient.Product?.Unit ?? ""}");
                }
            }

            DetailSteps.Items.Clear();

            if (recipe.Steps.Count == 0)
            {
                DetailSteps.Items.Add("Шаги приготовления не указаны.");
            }
            else
            {
                foreach (var step in recipe.Steps.OrderBy(s => s.StepNumber))
                {
                    DetailSteps.Items.Add($"{step.StepNumber}. {step.Description}");
                }
            }
        }

        private void BtnClone_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelectedRecipe();

            if (selected == null)
                return;

            var clone = new RecipePrototype(selected).Clone();

            var newName = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите название для копии рецепта:",
                "Создать копию",
                selected.Name + " (копия)");

            if (string.IsNullOrWhiteSpace(newName))
                return;

            clone.Name = newName.Trim();

            _recipeService.Add(clone);
            LoadRecipesFromDatabase();

            MessageBox.Show($"Копия «{clone.Name}» создана.", "Cook&Plan");
        }

        private void BtnDeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelectedRecipe();

            if (selected == null)
                return;

            var result = MessageBox.Show(
                $"Удалить рецепт «{selected.Name}»?",
                "Cook&Plan",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            _recipeService.Delete(selected.Id);

            RecipeDetails.Visibility = Visibility.Collapsed;
            LoadRecipesFromDatabase();

            MessageBox.Show("Рецепт удалён.", "Cook&Plan");
        }

        private void BtnAddMeal_Click(object sender, RoutedEventArgs e)
        {
            if (CmbMealRecipe.SelectedIndex < 0 || CmbMealRecipe.SelectedIndex >= _recipes.Count)
            {
                MessageBox.Show("Выберите рецепт.", "Cook&Plan");
                return;
            }

            var recipe = _recipes[CmbMealRecipe.SelectedIndex];

            var item = new UiMealPlanItem
            {
                Day = CmbDay.SelectedItem?.ToString() ?? "День",
                MealType = CmbMealType.SelectedItem?.ToString() ?? "Приём пищи",
                Recipe = recipe
            };

            _mealPlanItems.Add(item);
            RefreshMealPlanList();
        }

        private void BtnGenerateAutoPlan_Click(object sender, RoutedEventArgs e)
        {
            var result = _mealPlanningFacade.GenerateWeeklyPlanAndShoppingList();

            MealPlanList.Items.Clear();

            foreach (var line in result.PlanLines)
            {
                MealPlanList.Items.Add(line);
            }

            ShoppingListBox.Items.Clear();

            foreach (var line in result.ShoppingListLines)
            {
                ShoppingListBox.Items.Add(line);
            }

            MessageBox.Show("Пример недельного плана создан.", "Cook&Plan");
        }

        private void BtnClearMealPlan_Click(object sender, RoutedEventArgs e)
        {
            _mealPlanItems.Clear();
            MealPlanList.Items.Clear();
        }

        private void RefreshMealPlanList()
        {
            MealPlanList.Items.Clear();

            foreach (var group in _mealPlanItems.GroupBy(x => x.Day))
            {
                MealPlanList.Items.Add(group.Key);

                foreach (var item in group)
                {
                    MealPlanList.Items.Add($"   {item.MealType}: {item.Recipe.Name}");
                }

                MealPlanList.Items.Add("");
            }
        }

        private void BtnCreateList_Click(object sender, RoutedEventArgs e)
        {
            if (CmbRecipes.SelectedIndex < 0 || CmbRecipes.SelectedIndex >= _recipes.Count)
            {
                MessageBox.Show("Выберите рецепт.", "Cook&Plan");
                return;
            }

            var recipe = _recipes[CmbRecipes.SelectedIndex];
            var shoppingList = new SingleRecipeShoppingListFactory().Create(recipe);

            ShoppingListBox.Items.Clear();
            ShoppingListBox.Items.Add($"Список покупок для рецепта «{recipe.Name}»");
            ShoppingListBox.Items.Add("");

            foreach (var item in shoppingList.Items)
            {
                var ingredient = recipe.Ingredients.FirstOrDefault(i => i.ProductId == item.ProductId);

                ShoppingListBox.Items.Add(
                    $"• {ingredient?.Product?.Name ?? $"Продукт #{item.ProductId}"} — {item.Amount} {ingredient?.Product?.Unit ?? ""}");
            }
        }

        private void BtnCreatePlanShoppingList_Click(object sender, RoutedEventArgs e)
        {
            if (_mealPlanItems.Count == 0)
            {
                MessageBox.Show("Сначала добавьте рецепты в план питания.", "Cook&Plan");
                return;
            }

            var week = new WeeklyPlan("План на неделю");

            foreach (var dayGroup in _mealPlanItems.GroupBy(x => x.Day))
            {
                var dailyPlan = new DailyPlan(dayGroup.Key);

                foreach (var item in dayGroup)
                {
                    dailyPlan.Add(new CompositeRecipe(item.Recipe));
                }

                week.Add(dailyPlan);
            }

            var shoppingList = new CompositeShoppingListFactory().Create(week);

            ShoppingListBox.Items.Clear();
            ShoppingListBox.Items.Add("Список покупок для недельного плана");
            ShoppingListBox.Items.Add("");

            var planRecipes = _mealPlanItems.Select(x => x.Recipe).ToList();

            foreach (var item in shoppingList.Items)
            {
                var ingredient = planRecipes
                    .SelectMany(r => r.Ingredients)
                    .FirstOrDefault(i => i.ProductId == item.ProductId);

                ShoppingListBox.Items.Add(
                    $"• {ingredient?.Product?.Name ?? $"Продукт #{item.ProductId}"} — {item.Amount} {ingredient?.Product?.Unit ?? ""}");
            }
        }

        private void BtnClearShoppingList_Click(object sender, RoutedEventArgs e)
        {
            ShoppingListBox.Items.Clear();
        }

        private void BtnLoadJsonFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Выберите файл с рецептами"
            };

            if (dialog.ShowDialog() != true)
                return;

            TxtExternalRecipeJson.Text = File.ReadAllText(dialog.FileName, Encoding.UTF8);
        }

        private void BtnImportExternalRecipe_Click(object sender, RoutedEventArgs e)
        {
            var json = TxtExternalRecipeJson.Text.Trim();

            if (string.IsNullOrWhiteSpace(json))
            {
                MessageBox.Show("Вставьте JSON или выберите файл.", "Cook&Plan");
                return;
            }

            try
            {
                var importedRecipes = _recipeImportService.ImportAndSaveMany(json);

                LoadRecipesFromDatabase();

                ImportResultList.Items.Clear();
                ImportResultList.Items.Add($"Импортировано рецептов: {importedRecipes.Count}");
                ImportResultList.Items.Add("");

                foreach (var recipe in importedRecipes)
                {
                    ImportResultList.Items.Add($"• {recipe.Name} — {recipe.CookingTimeMinutes} мин, {recipe.Servings} порц.");
                }

                MessageBox.Show("Импорт завершён.", "Cook&Plan");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка импорта: {ex.Message}", "Cook&Plan");
            }
        }

        private void BtnClearImport_Click(object sender, RoutedEventArgs e)
        {
            TxtExternalRecipeJson.Clear();
            ImportResultList.Items.Clear();
        }

        private void LightBtn_Click(object sender, RoutedEventArgs e)
        {
            _themeFactory = new LightThemeFactory();
            ApplyTheme();
        }

        private void DarkBtn_Click(object sender, RoutedEventArgs e)
        {
            _themeFactory = new DarkThemeFactory();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            var windowStyle = _themeFactory.CreateWindowStyle();
            var buttonStyle = _themeFactory.CreateButtonStyle();

            _currentFg = windowStyle.Foreground;
            _currentBg = windowStyle.Background;

            var windowBg = Brush(windowStyle.Background);
            var windowFg = Brush(windowStyle.Foreground);
            var cardBg = Brush(buttonStyle.Background);
            var buttonBg = Brush(buttonStyle.Background);
            var buttonFg = Brush(buttonStyle.Foreground);
            var border = Brush(buttonStyle.BorderColor);

            RootWindow.Background = windowBg;
            HeaderPanel.Background = cardBg;
            MainTabs.Background = windowBg;

            foreach (var panel in new[]
            {
                RecipesPanel,
                MealPlanPanel,
                ShoppingPanel,
                ImportPanel
            })
            {
                panel.Background = windowBg;
            }

            foreach (var card in new[]
            {
                CreateRecipeCard,
                RecipeListCard,
                RecipeDetailsCard,
                MealPlanFormCard,
                MealPlanListCard,
                ShoppingFormCard,
                ShoppingResultCard,
                ImportJsonCard,
                ImportResultCard
            })
            {
                card.Background = cardBg;
                card.BorderBrush = border;
            }

            foreach (var button in new[]
            {
                LightBtn,
                DarkBtn,
                BtnAddIngr,
                BtnAddStep,
                BtnCreate,
                BtnClone,
                BtnDeleteRecipe,
                BtnAddMeal,
                BtnGenerateAutoPlan,
                BtnClearMealPlan,
                BtnCreateList,
                BtnCreatePlanShoppingList,
                BtnClearShoppingList,
                BtnLoadJsonFile,
                BtnImportExternalRecipe,
                BtnClearImport
            })
            {
                button.Background = buttonBg;
                button.Foreground = buttonFg;
                button.BorderBrush = border;
            }

            foreach (var textBox in new[]
            {
                TxtName,
                TxtDesc,
                TxtTime,
                TxtServings,
                TxtIngrName,
                TxtIngrAmount,
                TxtIngrUnit,
                TxtStep,
                TxtExternalRecipeJson
            })
            {
                textBox.Background = cardBg;
                textBox.Foreground = windowFg;
                textBox.BorderBrush = border;
                textBox.CaretBrush = windowFg;
            }

            foreach (var comboBox in new[]
            {
                CmbRecipeMealType,
                CmbRecipeCuisine,
                CmbRecipeDifficulty,
                CmbDay,
                CmbMealType,
                CmbMealRecipe,
                CmbRecipes
            })
            {
                comboBox.Background = cardBg;
                comboBox.Foreground = windowFg;
                comboBox.BorderBrush = border;
            }

            foreach (var listBox in new[]
            {
                RecipeList,
                IngrList,
                StepList,
                DetailIngredients,
                DetailSteps,
                MealPlanList,
                ShoppingListBox,
                ImportResultList
            })
            {
                listBox.Background = cardBg;
                listBox.Foreground = windowFg;
                listBox.BorderBrush = border;
            }

            foreach (var textBlock in new[]
            {
                HeaderTitle,
                HeaderSubtitle,
                LblCreateRecipe,
                LblName,
                LblDesc,
                LblRecipeMealType,
                LblRecipeCuisine,
                LblRecipeDifficulty,
                LblTime,
                LblServings,
                LblIngredients,
                LblSteps,
                LblRecipeList,
                DetailName,
                DetailInfo,
                DetailDescription,
                LblDetailIngredients,
                LblDetailSteps,
                LblMealPlanTitle,
                LblDay,
                LblMealType,
                LblMealRecipe,
                LblWeekPlan,
                LblShoppingTitle,
                LblShoppingHint,
                LblSelectRecipe,
                LblShoppingResult,
                LblImportTitle,
                LblImportHint,
                LblImportResult
            })
            {
                textBlock.Foreground = windowFg;
            }
        }

        private SolidColorBrush Brush(string hex)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }

        private class UiMealPlanItem
        {
            public string Day { get; set; } = string.Empty;
            public string MealType { get; set; } = string.Empty;
            public Recipe Recipe { get; set; } = null!;
        }
    }
}