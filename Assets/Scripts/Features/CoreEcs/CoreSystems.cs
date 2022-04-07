namespace Core
{
    public class CoreSystems : Feature
    {
        public ToolsSystem tools;

        public CoreSystems(Contexts contexts, CtxComponent ctx)
        {
            Add(new InputSystem(contexts));
            Add(new DragLogicSystem(contexts, ctx));
            // Add(new ProcessClickSystem(contexts));
            // Add(new CheatClickSystem(contexts)); 
            Add(new CoreCheatSystem(contexts));
            Add(new CreateLevelSystem(contexts, ctx));
            // Add(new FallSystem(contexts));
            // Add(new NearMatchSystem(contexts));
            //
            //
            // Add(new GenerateSystem(contexts));
            // Add(new SortingYSystem(contexts));
            // // Add(new TestFallSystem(contexts));
            tools = new ToolsSystem(contexts, ctx);
            Add(tools);
            //
            //
            // Add(new ChangeChipIconSystem(contexts));
            // Add(new EndMoveSystem(contexts));
            
            Add(new ChipsEventSystems(contexts));
            Add(new BoardEventSystems(contexts));
            Add(new GameEventSystems(contexts));
            
            Add(new LifeFlySystem(contexts));
            
            Add(new ChipsCleanupSystems(contexts));
            // Add(new BoardCleanupSystems(contexts));
            Add(new GameCleanupSystems(contexts));
            
            // Add(new TearDownSystem(contexts));
        }
    }
}