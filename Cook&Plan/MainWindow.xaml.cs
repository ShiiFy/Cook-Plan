using Cook_Plan.Core.Builders;
using Cook_Plan.Core.Composite;
using Cook_Plan.Core.Factories;
using Cook_Plan.Core.Prototypes;
using Cook_Plan.Core.Services;
using Cook_Plan.Core.Temporarily;
using Cook_Plan.Domain.Enums;
using Cook_Plan.Domain.Models;
using Cook_Plan.Themes;
using Cook_Plan.Themes.Dark;
using Cook_Plan.Themes.Light;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cook_Plan
{
    public partial class MainWindow : Window
    {
        private IThemeFactory _themeFactory = new LightThemeFactory();

        private readonly RecipeService _recipeService;

        private List<Recipe> _recipes = new();
        private List<MealPlanEntry> _mealPlanEntries = new();
        private List<Ingredient> _newIngredients = new();
        private List<RecipeStep> _newSteps = new();

        private string _currentFg = "#212121";
        private string _currentBg = "#FFFFFF";

        public MainWindow(RecipeService recipeService)
        {
            InitializeComponent();
            _recipeService = recipeService;

            EnsureDatabaseHasData();
            LoadRecipesFromDatabase();
            UpdateCacheInfo();
            ApplyTheme();
        }

        private void EnsureDatabaseHasData()
        {
            var existingRecipes = _recipeService.GetAll();
            if (existingRecipes.Count > 0) return;

            var dir = new RecipeDirector(new RecipeBuilder());

            var pasta = dir.Construct(
                "Паста Карбонара",
                "Классическая итальянская паста",
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
                    new() { StepNumber = 1, Description = "Отварить спагетти до аль денте" },
                    new() { StepNumber = 2, Description = "Обжарить бекон на сковороде" }
                });

            var omelette = dir.Construct(
                "Омлет",
                "Быстрый завтрак за 10 минут",
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
                    new() { StepNumber = 1, Description = "Взбить яйца с молоком и солью" }
                });

            _recipeService.Add(pasta);
            _recipeService.Add(omelette);
        }

        private void LoadRecipesFromDatabase()
        {
            _recipes = _recipeService.GetAll();
            RefreshRecipeList();
        }

        private void UpdateCacheInfo()
        {
            var cachedItems = RecipeCacheManager.GetAllCached();

            CacheInstanceInfo.Text =
                $"RecipeCacheManager → HashCode: {RecipeCacheManager.GetCacheHashCode()}\n" +
                $"Рецептов в кэше: {cachedItems.Count} из {_recipes.Count}\n" +
                $"Каждый вызов возвращает один и тот же объект = Singleton ✅";
        }

        private void RefreshRecipeList()
        {
            RecipeList.Items.Clear();
            CmbRecipes.Items.Clear();
            CmbMealRecipe.Items.Clear();
            CmbCacheRecipe.Items.Clear();

            foreach (var r in _recipes)
            {
                var line = $"[{r.Id}] {r.Name} — {r.CookingTimeMinutes} мин, {r.Servings} порц.";

                RecipeList.Items.Add(line);
                CmbRecipes.Items.Add($"[{r.Id}] {r.Name}");
                CmbMealRecipe.Items.Add($"[{r.Id}] {r.Name}");
                CmbCacheRecipe.Items.Add($"[{r.Id}] {r.Name}");
            }

            if (CmbCacheRecipe.Items.Count > 0) CmbCacheRecipe.SelectedIndex = 0;
            if (CmbRecipes.Items.Count > 0) CmbRecipes.SelectedIndex = 0;
            if (CmbMealRecipe.Items.Count > 0) CmbMealRecipe.SelectedIndex = 0;
        }

        private void BtnAddIngr_Click(object sender, RoutedEventArgs e)
        {
            var name = TxtIngrName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return;

            double.TryParse(TxtIngrAmount.Text, out double amount);

            _newIngredients.Add(new Ingredient
            {
                ProductId = _newIngredients.Count + 1,
                Amount = amount,
                Product = new Product
                {
                    Name = name,
                    Unit = "г"
                }
            });

            IngrList.Items.Add(new ListBoxItem
            {
                Content = $"• {name} — {amount} г",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_currentFg))
            });

            TxtIngrName.Clear();
            TxtIngrAmount.Text = "100";
        }

        private void BtnAddStep_Click(object sender, RoutedEventArgs e)
        {
            var desc = TxtStep.Text.Trim();

            if (string.IsNullOrWhiteSpace(desc))
                return;

            int num = _newSteps.Count + 1;

            _newSteps.Add(new RecipeStep
            {
                StepNumber = num,
                Description = desc
            });

            StepList.Items.Add(new ListBoxItem
            {
                Content = $"{num}. {desc}",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_currentFg))
            });

            TxtStep.Clear();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                MessageBox.Show("Введите название рецепта!", "Ошибка");
                return;
            }

            int.TryParse(TxtTime.Text, out int time);
            int.TryParse(TxtServings.Text, out int servings);

            var recipe = new RecipeBuilder()
                .SetBasicInfo(TxtName.Text.Trim(), TxtDesc.Text.Trim())
                .SetDetails(MealType.Dinner, Cuisine.Other, Difficulty.Easy)
                .SetCookingInfo(time > 0 ? time : 30, servings > 0 ? servings : 1)
                .AddIngredients(_newIngredients)
                .AddSteps(_newSteps)
                .Build();

            _recipeService.Add(recipe);

            LoadRecipesFromDatabase();
            UpdateCacheInfo();

            TxtName.Clear();
            TxtDesc.Clear();
            TxtTime.Text = "30";
            TxtServings.Text = "2";

            _newIngredients = new();
            _newSteps = new();

            IngrList.Items.Clear();
            StepList.Items.Clear();

            MessageBox.Show($"✅ Рецепт «{recipe.Name}» сохранен в Базу Данных!", "Успешно");
        }

        private void RecipeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = RecipeList.SelectedIndex;

            BtnClone.IsEnabled = idx >= 0;

            if (idx < 0)
            {
                RecipeDetails.Visibility = Visibility.Collapsed;
                return;
            }

            int selectedId = _recipes[idx].Id;

            var r = _recipeService.GetById(selectedId);

            if (r == null)
                return;

            RecipeDetails.Visibility = Visibility.Visible;

            DetailName.Text = $"🍽 {r.Name}";
            DetailInfo.Text =
                $"{r.MealType} | {r.Cuisine} | {r.Difficulty} | ⏱ {r.CookingTimeMinutes} мин | 🍽 {r.Servings} порц.";

            var fg = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_currentFg));
            var bg = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_currentBg));

            DetailIngredients.Items.Clear();

            foreach (var s in r.Ingredients.Count > 0
                ? r.Ingredients.Select(i => $"• {i.Product?.Name ?? "?"} — {i.Amount} {i.Product?.Unit ?? ""}")
                : new[] { "— нет ингредиентов —" })
            {
                DetailIngredients.Items.Add(new ListBoxItem
                {
                    Content = s,
                    Foreground = fg,
                    Background = bg
                });
            }

            DetailSteps.Items.Clear();

            foreach (var s in r.Steps.Count > 0
                ? r.Steps.Select(step => $"{step.StepNumber}. {step.Description}")
                : new[] { "— нет шагов —" })
            {
                DetailSteps.Items.Add(new ListBoxItem
                {
                    Content = s,
                    Foreground = fg,
                    Background = bg
                });
            }

            UpdateCacheInfo();
        }

        private void BtnClone_Click(object sender, RoutedEventArgs e)
        {
            var original = _recipes[RecipeList.SelectedIndex];
            var clone = new RecipePrototype(original).Clone();

            var newName = Microsoft.VisualBasic.Interaction.InputBox(
                $"Клонирован: «{original.Name}»\nВведите название для копии:",
                "Prototype — клонирование",
                original.Name + " (копия)");

            if (string.IsNullOrWhiteSpace(newName))
                return;

            clone.Name = newName;

            _recipeService.Add(clone);

            LoadRecipesFromDatabase();
            UpdateCacheInfo();

            MessageBox.Show($"🔁 Клонирован и сохранен в БД!\nКопия: «{clone.Name}»", "Prototype 🔁");
        }

        private void BtnCreateList_Click(object sender, RoutedEventArgs e)
        {
            if (CmbRecipes.SelectedIndex < 0)
                return;

            var recipe = _recipes[CmbRecipes.SelectedIndex];
            var list = new SingleRecipeShoppingListFactory().Create(recipe);

            ShoppingListBox.Items.Clear();
            ShoppingListBox.Items.Add($"📋 «{recipe.Name}»");
            ShoppingListBox.Items.Add("Factory использует CompositeRecipe внутри себя");
            ShoppingListBox.Items.Add("─────────────────────────────────");

            foreach (var item in list.Items)
            {
                var product = recipe.Ingredients
                    .FirstOrDefault(i => i.ProductId == item.ProductId)?
                    .Product;

                ShoppingListBox.Items.Add(
                    $"• {product?.Name ?? $"Продукт #{item.ProductId}"} — {item.Amount} {product?.Unit ?? ""}");
            }
        }

        private void BtnAddMeal_Click(object sender, RoutedEventArgs e)
        {
            if (CmbMealRecipe.SelectedIndex < 0)
                return;

            var recipe = _recipes[CmbMealRecipe.SelectedIndex];

            var day = (CmbDay.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "День";
            var mealType = (CmbMealType.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Приём пищи";

            _mealPlanEntries.Add(new MealPlanEntry
            {
                Id = _mealPlanEntries.Count + 1,
                Recipe = recipe
            });

            MealPlanList.Items.Add($"{day} / {mealType}: {recipe.Name}");
        }

        private void BtnCreateMealPlanList_Click(object sender, RoutedEventArgs e)
        {
            if (_mealPlanEntries.Count == 0)
            {
                MessageBox.Show("Сначала добавьте рецепты в план питания.");
                return;
            }

            var mealPlan = new MealPlan
            {
                Id = 1,
                Entries = _mealPlanEntries
            };

            var list = new MealPlanShoppingListFactory().Create(mealPlan);

            MealPlanShoppingList.Items.Clear();
            MealPlanShoppingList.Items.Add($"📋 Список из плана ({_mealPlanEntries.Count} записей)");
            MealPlanShoppingList.Items.Add("MealPlanShoppingListFactory использует WeeklyPlan + CompositeRecipe");
            MealPlanShoppingList.Items.Add("─────────────────────────────────");

            foreach (var item in list.Items)
            {
                var ingredient = _mealPlanEntries
                    .SelectMany(e => e.Recipe?.Ingredients ?? new List<Ingredient>())
                    .FirstOrDefault(i => i.ProductId == item.ProductId);

                MealPlanShoppingList.Items.Add(
                    $"• {ingredient?.Product?.Name ?? $"Продукт #{item.ProductId}"} — {item.Amount} {ingredient?.Product?.Unit ?? ""}");
            }
        }

        private void BtnTestCompositeFactory_Click(object sender, RoutedEventArgs e)
        {
            if (_recipes.Count == 0)
            {
                MessageBox.Show("Нет рецептов для проверки Composite + Factory.");
                return;
            }

            var recipe1 = _recipes[0];
            var recipe2 = _recipes.Count > 1 ? _recipes[1] : _recipes[0];

            var monday = new DailyPlan("Понедельник");
            monday.Add(new CompositeRecipe(recipe1));
            monday.Add(new CompositeRecipe(recipe2));

            var tuesday = new DailyPlan("Вторник");
            tuesday.Add(new CompositeRecipe(recipe1));

            var week = new WeeklyPlan("Тестовая неделя");
            week.Add(monday);
            week.Add(tuesday);

            var factory = new CompositeShoppingListFactory();
            var shoppingList = factory.Create(week);

            CompositeFactoryResultBox.Items.Clear();

            CompositeFactoryResultBox.Items.Add("✅ Проверка Composite + Factory");
            CompositeFactoryResultBox.Items.Add("─────────────────────────────────");
            CompositeFactoryResultBox.Items.Add("Структура:");
            CompositeFactoryResultBox.Items.Add("WeeklyPlan");
            CompositeFactoryResultBox.Items.Add(" ├─ DailyPlan: Понедельник");
            CompositeFactoryResultBox.Items.Add($" │   ├─ CompositeRecipe: {recipe1.Name}");
            CompositeFactoryResultBox.Items.Add($" │   └─ CompositeRecipe: {recipe2.Name}");
            CompositeFactoryResultBox.Items.Add(" └─ DailyPlan: Вторник");
            CompositeFactoryResultBox.Items.Add($"     └─ CompositeRecipe: {recipe1.Name}");
            CompositeFactoryResultBox.Items.Add("─────────────────────────────────");
            CompositeFactoryResultBox.Items.Add("Результат ShoppingList:");

            foreach (var item in shoppingList.Items)
            {
                var ingredient = new[] { recipe1, recipe2 }
                    .SelectMany(r => r.Ingredients)
                    .FirstOrDefault(i => i.ProductId == item.ProductId);

                CompositeFactoryResultBox.Items.Add(
                    $"• {ingredient?.Product?.Name ?? $"Продукт #{item.ProductId}"} — {item.Amount} {ingredient?.Product?.Unit ?? ""}");
            }

            CompositeFactoryResultBox.Items.Add("─────────────────────────────────");
            CompositeFactoryResultBox.Items.Add("Composite собрал ингредиенты.");
            CompositeFactoryResultBox.Items.Add("Factory создала ShoppingList.");
        }

        private void BtnCheckCache_Click(object sender, RoutedEventArgs e)
        {
            var cachedItems = RecipeCacheManager.GetAllCached();

            CacheLog.Items.Clear();
            CacheLog.Items.Add($"=== СОСТОЯНИЕ КЭША (HashCode: {RecipeCacheManager.GetCacheHashCode()}) ===");

            if (cachedItems.Count == 0)
            {
                CacheLog.Items.Add("📭 Кэш пуст. Следующий запрос пойдет в БД.");
            }
            else
            {
                foreach (var item in cachedItems)
                    CacheLog.Items.Add($"✅ [ID: {item.Id}] {item.Name}");
            }
        }

        private void BtnClearCache_Click(object sender, RoutedEventArgs e)
        {
            var idx = CmbCacheRecipe.SelectedIndex;

            if (idx >= 0)
            {
                var r = _recipes[idx];

                _recipeService.Delete(r.Id);

                LoadRecipesFromDatabase();

                CacheLog.Items.Add($"🗑️ id={r.Id} «{r.Name}» удалён из БД и кэша!");

                UpdateCacheInfo();
            }
        }

        private void BtnReloadCache_Click(object sender, RoutedEventArgs e)
        {
            LoadRecipesFromDatabase();

            CacheLog.Items.Add("🔄 Данные синхронизированы с БД.");

            UpdateCacheInfo();
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
            var w = _themeFactory.CreateWindowStyle();
            var b = _themeFactory.CreateButtonStyle();

            _currentFg = w.Foreground;
            _currentBg = b.Background;

            var winBg = Brush(w.Background);
            var winFg = Brush(w.Foreground);
            var btnBg = Brush(b.Background);
            var btnFg = Brush(b.Foreground);
            var border = Brush(b.BorderColor);

            RootWindow.Background = winBg;
            HeaderPanel.Background = btnBg;
            LeftPanel.Background = winBg;
            RightPanel.Background = winBg;
            ShoppingPanel.Background = winBg;
            CachePanel.Background = winBg;
            MealPlanPanel.Background = winBg;
            CacheInfoBox.Background = btnBg;
            CacheInfoBox.BorderBrush = border;
            MainTabs.Background = winBg;

            foreach (var btn in new[]
            {
                LightBtn,
                DarkBtn,
                BtnCreate,
                BtnClone,
                BtnAddIngr,
                BtnAddStep,
                BtnCreateList,
                BtnCheckCache,
                BtnClearCache,
                BtnReloadCache,
                BtnAddMeal,
                BtnCreateMealPlanList,
                BtnTestCompositeFactory
            })
            {
                btn.Background = btnBg;
                btn.Foreground = btnFg;
                btn.BorderBrush = border;
            }

            foreach (var tb in new[]
            {
                TxtName,
                TxtDesc,
                TxtTime,
                TxtServings,
                TxtIngrName,
                TxtIngrAmount,
                TxtStep
            })
            {
                tb.Background = btnBg;
                tb.Foreground = winFg;
                tb.BorderBrush = border;
                tb.CaretBrush = winFg;
            }

            foreach (var cb in new[]
            {
                CmbRecipes,
                CmbMealRecipe,
                CmbDay,
                CmbMealType,
                CmbCacheRecipe
            })
            {
                cb.Background = btnBg;
                cb.Foreground = winFg;
                cb.BorderBrush = border;

                foreach (var item in cb.Items.OfType<ComboBoxItem>())
                {
                    item.Background = btnBg;
                    item.Foreground = winFg;
                }
            }

            foreach (var lb in new[]
            {
                RecipeList,
                IngrList,
                StepList,
                ShoppingListBox,
                CacheLog,
                MealPlanList,
                MealPlanShoppingList,
                DetailIngredients,
                DetailSteps,
                CompositeFactoryResultBox
            })
            {
                lb.Background = btnBg;
                lb.Foreground = winFg;
                lb.BorderBrush = border;
            }

            foreach (var tb in new TextBlock[]
            {
                BuilderLabel,
                PrototypeLabel,
                FactoryLabel,
                SingletonLabel,
                MealPlanLabel,
                MealPlanFactoryLabel,
                LblName,
                LblDesc,
                LblTime,
                LblServings,
                LblIngr,
                LblSteps,
                LblSelectRecipe,
                LblResult,
                LblLog,
                LblCacheInfo,
                LblCacheSelect,
                LblDetailIngr,
                LblDetailSteps,
                LblDay,
                LblMealType,
                LblMealRecipe,
                LblPlanEntries,
                LblMealPlanResult,
                HeaderTitle,
                CacheInstanceInfo,
                DetailName,
                DetailInfo,
                CompositeFactoryLabel,
                CompositeFactoryDescription,
                LblCompositeFactoryResult
            })
            {
                tb.Foreground = winFg;
            }

            RefreshDetailColors(winFg, btnBg);
        }

        private void RefreshDetailColors(SolidColorBrush fg, SolidColorBrush bg)
        {
            foreach (var lb in new[]
            {
                DetailIngredients,
                DetailSteps,
                IngrList,
                StepList
            })
            {
                foreach (var item in lb.Items.OfType<ListBoxItem>())
                {
                    item.Foreground = fg;
                    item.Background = bg;
                }
            }
        }

        private SolidColorBrush Brush(string hex)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }
    }
}