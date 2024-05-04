using ImGuiNET;
using Swed64;
using ClickableTransparentOverlay;
using System.Numerics;
using System.Runtime.InteropServices;

namespace External
{
    class Program : Overlay
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        bool Window = true;
        bool UnlimitedCoins = false;
        bool UnlimitedGems = false;
        bool InfFuel = false;
        int Gems;
        int Coins;
        int PreviousGems;
        int PreviousCoins;
        IntPtr module;
        int CoinsAddr = 0xC0E21;
        int CoinsAdd = 0x28CAD4;
        int GemsAddr = 0xCD00C;
        int GemsAdd = 0x28CAEC;
        int FuelAddr = 0x0028CA2C;
        int FuelOffset = 0x2A8;
        Swed swed = new Swed("HillClimbRacing");

        protected override void Render()
        {

            if (GetAsyncKeyState(0x2D)<0)
            {
                Window = !Window;
                Thread.Sleep(250);
            }

            if (Window)
            {
                ImGuiStylePtr style = ImGui.GetStyle();
                style.FrameBorderSize = 1.0f;
                style.WindowRounding = 0.0f;
                style.ChildRounding = 0.0f;
                style.FrameRounding = 0.0f;
                style.PopupRounding = 0.0f;
                style.ScrollbarRounding = 0.0f;
                style.GrabRounding = 0.0f;
                style.TabRounding = 0.0f;

                style.Colors[(int)ImGuiCol.Text] = new Vector4(1.0f, 1.0f, 1.0f, 1.00f);
                style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
                style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.09f, 0.09f, 0.09f, 0.94f);
                style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.11f, 0.11f, 0.11f, 1.00f);
                style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.11f, 0.11f, 0.11f, 0.94f);
                style.Colors[(int)ImGuiCol.Border] = new Vector4(0.07f, 0.08f, 0.08f, 1.00f);
                style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
                style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.35f, 0.35f, 0.35f, 0.54f);
                style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.31f, 0.29f, 0.27f, 1.00f);
                style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.40f, 0.36f, 0.33f, 0.67f);
                style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.15f, 0.15f, 0.15f, 0.9f);
                style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.15f, 0.15f, 0.15f, 0.9f);
                style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.15f, 0.15f, 0.15f, 0.9f);
                style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.18f, 0.18f, 0.18f, 0.94f);
                style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.16f);
                style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.24f, 0.22f, 0.21f, 1.00f);
                style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.31f, 0.29f, 0.27f, 1.00f);
                style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.40f, 0.36f, 0.33f, 1.00f);
                style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.8f, 0.8f, 0.8f, 1.0f);
                style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.55f, 0.55f, 0.55f, 1.00f);
                style.Colors[(int)ImGuiCol.Button] = new Vector4(0.35f, 0.35f, 0.35f, 0.54f);
                style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.25f, 0.25f, 0.25f, 0.62f);
                style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.60f, 0.60f, 0.60f, 1.00f);
                style.Colors[(int)ImGuiCol.Header] = new Vector4(0.84f, 0.36f, 0.05f, 0.0f);
                style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.25f, 0.25f, 0.25f, 0.80f);
                style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.42f, 0.42f, 0.42f, 1.00f);
                style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.35f, 0.35f, 0.35f, 0.50f);
                style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.31f, 0.29f, 0.27f, 0.78f);
                style.Colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.40f, 0.36f, 0.33f, 1.00f);
                style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(1.0f, 1.0f, 1.0f, 0.25f);
                style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(1.00f, 1.0f, 1.0f, 0.4f);
                style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(1.00f, 1.00f, 1.0f, 0.95f);
                style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.18f, 0.18f, 0.18f, 1.0f);
                style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.58f, 0.58f, 0.58f, 0.80f);
                style.Colors[(int)ImGuiCol.TabActive] = new Vector4(0.6f, 0.60f, 0.60f, 1.00f);
                style.Colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.07f, 0.10f, 0.15f, 0.97f);
                style.Colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.14f, 0.26f, 0.42f, 1.00f);
                style.Colors[(int)ImGuiCol.PlotLines] = new Vector4(0.66f, 0.60f, 0.52f, 1.00f);
                style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(0.98f, 0.29f, 0.20f, 1.00f);
                style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.60f, 0.59f, 0.10f, 1.00f);
                style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.72f, 0.73f, 0.15f, 1.00f);
                style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.27f, 0.52f, 0.53f, 0.35f);
                style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(0.60f, 0.59f, 0.10f, 0.90f);
                style.Colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.51f, 0.65f, 0.60f, 1.00f);
                style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
                style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.20f);
                style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.11f, 0.13f, 0.13f, 0.35f);

                ImGui.SetNextWindowSize(new Vector2(400, 250));

                ImGui.Begin("Main", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar);

                ImGui.SeparatorText("Hill Climb Cheat");

                //coins
                ImGui.Checkbox("Unlimited Coins", ref UnlimitedCoins);
                ImGui.SliderInt("Coins", ref Coins, 0, 999999999);

                //gems
                ImGui.Checkbox("Unlimited Gems", ref UnlimitedGems);
                ImGui.SliderInt("Gems", ref Gems, 0, 999999999);

                if (ImGui.Button("Apply Changes"))
                {
                    ApplyChanges();
                }

                ImGui.Checkbox("Inf Fuel", ref InfFuel);

                if (ImGui.Button("Exit"))
                    Environment.Exit(0);

                ImGui.Text("Made By ice_cream_sandwitch");

                ImGui.End();
            }
        }

        public void ApplyChanges()
        {
            if (Gems != PreviousGems)
            {
                swed.WriteInt(module, GemsAdd, Gems);
                PreviousGems = Gems;
            }

            if (Coins != PreviousCoins)
            {
                swed.WriteInt(module, CoinsAdd, Coins);
                PreviousCoins = Coins;
            }
        }

        public void Logic()
        {
            module = swed.GetModuleBase(".exe");

            while (true)
            {
                if (UnlimitedCoins)
                {
                    swed.WriteBytes(module, CoinsAddr, "90 90 90 90 90 90");
                }
                else
                {
                    swed.WriteBytes(module, CoinsAddr, "89 15 D4 CA 41 00");
                }

                if (UnlimitedGems)
                {
                    swed.WriteBytes(module, GemsAddr, "90 90 90 90 90 90");
                }
                else
                {
                    swed.WriteBytes(module, GemsAddr, "29 3D EC CA 41 00");
                }
                
                if (InfFuel)
                {
                    IntPtr FuelPtrAddr = swed.ReadPointer(module, FuelAddr) + FuelOffset;
                    swed.WriteFloat(FuelPtrAddr, 100);
                }
            }
        }


        static void Main(string[] args)
        {

            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
           
            Program program = new Program();
            program.Start().Wait();

            Thread threadHack = new Thread(program.Logic) { IsBackground = true };
            threadHack.Start();
        }
    }
}
