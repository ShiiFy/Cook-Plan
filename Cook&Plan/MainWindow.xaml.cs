using Cook_Plan.Core.Builders;
using Cook_Plan.Core.Cache;
using Cook_Plan.Core.Factories;
using Cook_Plan.Core.Prototypes;
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
        private List<Recipe> _recipes = new();
        private List<MealPlanEntry> _mealPlanEntries = new();
        private List<Ingredient> _newIngredients = new();
        private List<RecipeStep> _newSteps = new();
        private string _currentFg = "#212121";
        private string _currentBg = "#FFFFFF";

        public MainWindow()
        {
            InitializeComponent();
            SeedRecipes();
            LoadAllToCache();
            RefreshRecipeList();
            UpdateCacheInfo();
            ApplyTheme();
        }

        // ── Тестовые данные ───────────────────────────────────────────
        private void SeedRecipes()
        {
            var dir = new RecipeDirector(new RecipeBuilder());

            var pasta = dir.Construct("Паста Карбонара", "Классическая итальянская паста",
                MealType.Lunch, Cuisine.Italian, Difficulty.Medium, 25, 2,
                new List<Ingredient>
                {
                    new() { ProductId=1, Amount=200, Product=new Product{Name="Спагетти", Unit="г"  }},
                    new() { ProductId=2, Amount=100, Product=new Product{Name="Бекон",    Unit="г"  }},
                    new() { ProductId=3, Amount=2,   Product=new Product{Name="Яйца",     Unit="шт" }},
                    new() { ProductId=4, Amount=50,  Product=new Product{Name="Пармезан", Unit="г"  }},
                },
                new List<RecipeStep>
                {
                    new() { StepNumber=1, Description="Отварить спагетти до аль денте" },
                    new() { StepNumber=2, Description="Обжарить бекон на сковороде" },
                    new() { StepNumber=3, Description="Взбить яйца с пармезаном" },
                    new() { StepNumber=4, Description="Смешать пасту с беконом и яичной смесью" },
                });
            pasta.Id = 1;

            var omelette = dir.Construct("Омлет", "Быстрый завтрак за 10 минут",
                MealType.Breakfast, Cuisine.French, Difficulty.Easy, 10, 1,
                new List<Ingredient>
                {
                    new() { ProductId=3, Amount=3,  Product=new Product{Name="Яйца",   Unit="шт"}},
                    new() { ProductId=5, Amount=50, Product=new Product{Name="Молоко", Unit="мл"}},
                },
                new List<RecipeStep>
                {
                    new() { StepNumber=1, Description="Взбить яйца с молоком и солью" },
                    new() { StepNumber=2, Description="Разогреть сковороду с маслом" },
                    new() { StepNumber=3, Description="Вылить смесь и жарить 3–4 мин" },
                });
            omelette.Id = 2;

            var macaroni = dir.Construct("Макароны с сосисками", "Простое и сытное блюдо",
                MealType.Dinner, Cuisine.Other, Difficulty.Easy, 20, 2,
                new List<Ingredient>
                {
                    new() { ProductId=6, Amount=200, Product=new Product{Name="Макароны", Unit="г"  }},
                    new() { ProductId=7, Amount=3,   Product=new Product{Name="Сосиски",  Unit="шт" }},
                },
                new List<RecipeStep>
                {
                    new() { StepNumber=1, Description="Отварить макароны" },
                    new() { StepNumber=2, Description="Нарезать и обжарить сосиски" },
                    new() { StepNumber=3, Description="Смешать и подавать" },
                });
            macaroni.Id = 3;

            _recipes.AddRange(new[] { pasta, omelette, macaroni });
        }

        // ── Singleton: автозагрузка ────────────────────────────────────
        private void LoadAllToCache()
        {
            var cache = RecipeCache.GetIstanse();
            foreach (var r in _recipes)
                cache.Set(r);
        }

        private void UpdateCacheInfo()
        {
            var cache = RecipeCache.GetIstanse();
            int cached = _recipes.Count(r => cache.Contains(r.Id));
            CacheInstanceInfo.Text =
                $"RecipeCache.GetIstanse() → HashCode: {cache.GetHashCode()}\n" +
                $"Рецептов в кэше: {cached} из {_recipes.Count}\n" +
                $"Каждый вызов GetIstanse() возвращает один и тот же объект = Singleton ✅";
        }

        // ── Обновить списки ───────────────────────────────────────────
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

        // ── Builder: добавить ингредиент ──────────────────────────────
        private void BtnAddIngr_Click(object sender, RoutedEventArgs e)
        {
            var name = TxtIngrName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name)) return;
            double.TryParse(TxtIngrAmount.Text, out double amount);

            _newIngredients.Add(new Ingredient
            {
                ProductId = _newIngredients.Count + 1,
                Amount = amount,
                Product = new Product { Name = name, Unit = "г" }
            });
            IngrList.Items.Add(new ListBoxItem
            {
                Content = $"• {name} — {amount} г",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_currentFg))
            });
            TxtIngrName.Clear();
            TxtIngrAmount.Text = "100";
        }

        // ── Builder: добавить шаг ─────────────────────────────────────
        private void BtnAddStep_Click(object sender, RoutedEventArgs e)
        {
            var desc = TxtStep.Text.Trim();
            if (string.IsNullOrWhiteSpace(desc)) return;
            int num = _newSteps.Count + 1;
            _newSteps.Add(new RecipeStep { StepNumber = num, Description = desc });
            StepList.Items.Add(new ListBoxItem
            {
                Content = $"{num}. {desc}",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_currentFg))
            });
            TxtStep.Clear();
        }

        // ── Builder: создать рецепт ────────────────────────────────────
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            { MessageBox.Show("Введите название рецепта!", "Ошибка"); return; }

            int.TryParse(TxtTime.Text, out int time);
            int.TryParse(TxtServings.Text, out int servings);

            var recipe = new RecipeBuilder()
                .SetBasicInfo(TxtName.Text.Trim(), TxtDesc.Text.Trim())
                .SetDetails(MealType.Dinner, Cuisine.Other, Difficulty.Easy)
                .SetCookingInfo(time > 0 ? time : 30, servings > 0 ? servings : 1)
                .AddIngredients(_newIngredients)
                .AddSteps(_newSteps)
                .Build();

            recipe.Id = _recipes.Count + 1;
            _recipes.Add(recipe);
            RecipeCache.GetIstanse().Set(recipe);
            CacheLog.Items.Add($"📥 Авто-кэш: «{recipe.Name}» (id={recipe.Id})");

            RefreshRecipeList();
            UpdateCacheInfo();

            TxtName.Clear(); TxtDesc.Clear();
            TxtTime.Text = "30"; TxtServings.Text = "2";
            _newIngredients = new(); _newSteps = new();
            IngrList.Items.Clear(); StepList.Items.Clear();

            MessageBox.Show(
                $"✅ Рецепт «{recipe.Name}» создан через Builder!\n" +
                $"Ингредиентов: {recipe.Ingredients.Count}, Шагов: {recipe.Steps.Count}\n" +
                $"Автоматически добавлен в кэш.", "Builder ✅");
        }

        // ── Prototype: детали рецепта ─────────────────────────────────
        private void RecipeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = RecipeList.SelectedIndex;
            BtnClone.IsEnabled = idx >= 0;
            if (idx < 0) { RecipeDetails.Visibility = Visibility.Collapsed; return; }

            var r = _recipes[idx];
            RecipeDetails.Visibility = Visibility.Visible;
            DetailName.Text = $"🍽 {r.Name}";
            DetailInfo.Text = $"{r.MealType} | {r.Cuisine} | {r.Difficulty} | ⏱ {r.CookingTimeMinutes} мин | 🍽 {r.Servings} порц.";

            var fg = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_currentFg));
            var bg = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_currentBg));

            DetailIngredients.Items.Clear();
            foreach (var s in r.Ingredients.Count > 0
                ? r.Ingredients.Select(i => $"• {i.Product?.Name ?? "?"} — {i.Amount} {i.Product?.Unit}")
                : new[] { "— нет ингредиентов —" })
                DetailIngredients.Items.Add(new ListBoxItem { Content = s, Foreground = fg, Background = bg });

            DetailSteps.Items.Clear();
            foreach (var s in r.Steps.Count > 0
                ? r.Steps.Select(s => $"{s.StepNumber}. {s.Description}")
                : new[] { "— нет шагов —" })
                DetailSteps.Items.Add(new ListBoxItem { Content = s, Foreground = fg, Background = bg });
        }

        // ── Prototype: клонировать ────────────────────────────────────
        private void BtnClone_Click(object sender, RoutedEventArgs e)
        {
            var original = _recipes[RecipeList.SelectedIndex];
            var clone = new RecipePrototype(original).Clone();

            var newName = Microsoft.VisualBasic.Interaction.InputBox(
                $"Клонирован: «{original.Name}»\nВведите название для копии:",
                "Prototype — клонирование",
                original.Name + " (копия)");

            if (string.IsNullOrWhiteSpace(newName)) return;

            clone.Id = _recipes.Count + 1;
            clone.Name = newName;
            _recipes.Add(clone);
            RecipeCache.GetIstanse().Set(clone);
            CacheLog.Items.Add($"📥 Авто-кэш: клон «{clone.Name}» (id={clone.Id})");

            RefreshRecipeList();
            UpdateCacheInfo();

            MessageBox.Show(
                $"🔁 Клонирован через Prototype!\n" +
                $"Оригинал: «{original.Name}»\nКопия: «{clone.Name}»\n" +
                $"Ингредиентов: {clone.Ingredients.Count}, Шагов: {clone.Steps.Count}",
                "Prototype 🔁");
        }

        // ── Factory Method: одиночный рецепт ──────────────────────────
        private void BtnCreateList_Click(object sender, RoutedEventArgs e)
        {
            if (CmbRecipes.SelectedIndex < 0) { MessageBox.Show("Выберите рецепт!"); return; }

            var recipe = _recipes[CmbRecipes.SelectedIndex];
            var list = new SingleRecipeShoppingListFactory().Create(recipe);

            ShoppingListBox.Items.Clear();
            ShoppingListBox.Items.Add($"📋 «{recipe.Name}»");
            ShoppingListBox.Items.Add($"📅 {list.CreatedAt}  |  🏭 SingleRecipeShoppingListFactory");
            ShoppingListBox.Items.Add("─────────────────────────────────");
            if (list.Items.Count == 0)
                ShoppingListBox.Items.Add("— нет ингредиентов —");
            else
                foreach (var item in list.Items)
                {
                    var ingr = recipe.Ingredients.FirstOrDefault(i => i.ProductId == item.ProductId);
                    ShoppingListBox.Items.Add($"• {ingr?.Product?.Name ?? $"#{item.ProductId}"} — {item.Amount} {ingr?.Product?.Unit}");
                }

            MessageBox.Show($"🛒 Список создан!\nПозиций: {list.Items.Count}", "Factory Method 🏭");
        }

        // ── План питания ───────────────────────────────────────────────
        private void BtnAddMeal_Click(object sender, RoutedEventArgs e)
        {
            if (CmbMealRecipe.SelectedIndex < 0) { MessageBox.Show("Выберите рецепт!"); return; }

            var days = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                                   DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            var meals = new[] { MealType.Breakfast, MealType.Lunch, MealType.Dinner, MealType.Snack };
            var dayNames = new[] { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
            var mealNames = new[] { "Завтрак", "Обед", "Ужин", "Перекус" };

            var recipe = _recipes[CmbMealRecipe.SelectedIndex];
            _mealPlanEntries.Add(new MealPlanEntry
            {
                Id = _mealPlanEntries.Count + 1,
                DayOfWeek = days[CmbDay.SelectedIndex],
                MealType = meals[CmbMealType.SelectedIndex],
                RecipeId = recipe.Id,
                Recipe = recipe,
                Servings = 1
            });
            MealPlanList.Items.Add(
                $"{dayNames[CmbDay.SelectedIndex]} | {mealNames[CmbMealType.SelectedIndex]} | {recipe.Name}");
        }

        private void BtnCreateMealPlanList_Click(object sender, RoutedEventArgs e)
        {
            if (_mealPlanEntries.Count == 0)
            { MessageBox.Show("Добавьте хотя бы один приём пищи в план!"); return; }

            var mealPlan = new MealPlan
            {
                Id = 1,
                WeekStartDate = DateOnly.FromDateTime(DateTime.Today),
                Entries = _mealPlanEntries
            };
            var list = new MealPlanShoppingListFactory().Create(mealPlan);
            var allIngr = _mealPlanEntries.Where(e => e.Recipe != null)
                                          .SelectMany(e => e.Recipe!.Ingredients).ToList();

            MealPlanShoppingList.Items.Clear();
            MealPlanShoppingList.Items.Add($"📋 Список из плана питания ({_mealPlanEntries.Count} записей)");
            MealPlanShoppingList.Items.Add($"📅 {list.CreatedAt}  |  🏭 MealPlanShoppingListFactory");
            MealPlanShoppingList.Items.Add("─────────────────────────────────");
            if (list.Items.Count == 0)
                MealPlanShoppingList.Items.Add("— нет ингредиентов —");
            else
                foreach (var item in list.Items)
                {
                    var name = allIngr.FirstOrDefault(i => i.ProductId == item.ProductId)?.Product?.Name
                               ?? $"#{item.ProductId}";
                    MealPlanShoppingList.Items.Add($"• {name} — {item.Amount}");
                }

            MessageBox.Show($"🛒 Создан через MealPlanShoppingListFactory!\nПозиций: {list.Items.Count}",
                "Factory Method 🏭");
        }

        // ── Singleton: кэш — теперь для любого рецепта ────────────────
        private Recipe? GetSelectedCacheRecipe()
        {
            int idx = CmbCacheRecipe.SelectedIndex;
            return idx >= 0 ? _recipes[idx] : null;
        }

        private void BtnCheckCache_Click(object sender, RoutedEventArgs e)
        {
            var recipe = GetSelectedCacheRecipe();
            if (recipe == null) { MessageBox.Show("Выберите рецепт!"); return; }

            var cache = RecipeCache.GetIstanse();
            CacheLog.Items.Add(cache.Contains(recipe.Id)
                ? $"✅ id={recipe.Id} «{cache.Get(recipe.Id)?.Name}» есть в кэше  (HashCode: {cache.GetHashCode()})"
                : $"❌ id={recipe.Id} «{recipe.Name}» отсутствует в кэше  (HashCode: {cache.GetHashCode()})");
            UpdateCacheInfo();
        }

        private void BtnClearCache_Click(object sender, RoutedEventArgs e)
        {
            var recipe = GetSelectedCacheRecipe();
            if (recipe == null) { MessageBox.Show("Выберите рецепт!"); return; }

            RecipeCache.GetIstanse().Remove(recipe.Id);
            CacheLog.Items.Add($"🗑️ id={recipe.Id} «{recipe.Name}» удалён из кэша  (HashCode: {RecipeCache.GetIstanse().GetHashCode()})");
            UpdateCacheInfo();
        }

        private void BtnReloadCache_Click(object sender, RoutedEventArgs e)
        {
            LoadAllToCache();
            CacheLog.Items.Add($"🔄 Все рецепты ({_recipes.Count} шт.) перезагружены  (HashCode: {RecipeCache.GetIstanse().GetHashCode()})");
            UpdateCacheInfo();
        }

        // ��─ Abstract Factory: темы ────────────────────────────────────
        private void LightBtn_Click(object sender, RoutedEventArgs e)
        { _themeFactory = new LightThemeFactory(); ApplyTheme(); }

        private void DarkBtn_Click(object sender, RoutedEventArgs e)
        { _themeFactory = new DarkThemeFactory(); ApplyTheme(); }

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

            foreach (var btn in new[] { LightBtn, DarkBtn, BtnCreate, BtnClone,
                                        BtnAddIngr, BtnAddStep, BtnCreateList,
                                        BtnCheckCache, BtnClearCache, BtnReloadCache,
                                        BtnAddMeal, BtnCreateMealPlanList })
            {
                btn.Background = btnBg;
                btn.Foreground = btnFg;
                btn.BorderBrush = border;
            }

            foreach (var tb in new[] { TxtName, TxtDesc, TxtTime, TxtServings,
                                       TxtIngrName, TxtIngrAmount, TxtStep })
            {
                tb.Background = btnBg;
                tb.Foreground = winFg;
                tb.BorderBrush = border;
                tb.CaretBrush = winFg;
            }

            // ComboBox-ы — цвет + цвет элементов внутри
            foreach (var cb in new[] { CmbRecipes, CmbMealRecipe, CmbDay,
                                       CmbMealType, CmbCacheRecipe })
            {
                cb.Background = btnBg;
                cb.Foreground = winFg;
                cb.BorderBrush = border;
                // Перекрашиваем ComboBoxItem внутри
                foreach (var item in cb.Items.OfType<ComboBoxItem>())
                {
                    item.Background = btnBg;
                    item.Foreground = winFg;
                }
            }

            foreach (var lb in new[] { RecipeList, IngrList, StepList,
                                       ShoppingListBox, CacheLog,
                                       MealPlanList, MealPlanShoppingList,
                                       DetailIngredients, DetailSteps })
            {
                lb.Background = btnBg;
                lb.Foreground = winFg;
                lb.BorderBrush = border;
            }

            foreach (var tb in new TextBlock[]
            {
                BuilderLabel, PrototypeLabel, FactoryLabel, SingletonLabel,
                MealPlanLabel, MealPlanFactoryLabel,
                LblName, LblDesc, LblTime, LblServings, LblIngr, LblSteps,
                LblSelectRecipe, LblResult, LblLog, LblCacheInfo, LblCacheSelect,
                LblDetailIngr, LblDetailSteps, LblDay, LblMealType,
                LblMealRecipe, LblPlanEntries, LblMealPlanResult,
                HeaderTitle, CacheInstanceInfo, DetailName, DetailInfo
            })
                tb.Foreground = winFg;

            RefreshDetailColors(winFg, btnBg);
        }

        private void RefreshDetailColors(SolidColorBrush fg, SolidColorBrush bg)
        {
            foreach (var lb in new[] { DetailIngredients, DetailSteps, IngrList, StepList })
                foreach (var item in lb.Items.OfType<ListBoxItem>())
                { item.Foreground = fg; item.Background = bg; }
        }

        private SolidColorBrush Brush(string hex) =>
            new((Color)ColorConverter.ConvertFromString(hex));
    }
}