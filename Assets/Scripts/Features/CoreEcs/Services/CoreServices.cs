using Api.Merge;
using Data;
using Systems.Merge;

namespace Core
{
    public class CoreServices 
    {
        public LevelConfigService levelConfig;
        public CameraFitApi cameraFit;
        public GridGeneratorApi gridGenerator;
        public MergeLogicApi logicApi;
        public ViewFactoryService viewFactory;
        public FactoryService factory;
        public BoardService board;
        

        public void Init(
            Contexts contexts
            , CtxComponent ctx
            , MergePlayerLinks view
            , MergeConfig config
            , MergeDynamicData data
            )
        {
            levelConfig = new LevelConfigService(config);
            cameraFit = new CameraFitApi();
            gridGenerator = new GridGeneratorApi(view.GridGeneratorLinks);
            logicApi = new MergeLogicApi();
            viewFactory = new ViewFactoryService();
            factory = new FactoryService(contexts, config, viewFactory);
            board = new BoardService(contexts, config, factory, viewFactory);
            
            viewFactory.SetCtx(view.Container, view.Camera, config);
            cameraFit.SetCtx(view.Camera, view.CameraTransform);
            logicApi.SetCtx(contexts, data, viewFactory);
        }
    }
}