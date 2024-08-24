
namespace GDC.Enums
{
    public enum DialogueState
    {
        HEAD,
        BRANCH,
        TAIL,
    }
    public enum TransitionType
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN,
        IN,
        FADE,
    }
    public enum TransitionLoadSceneType
    {
        NEW_SCENE, //Load sang scene moi
        RELOAD_WITH_TRANSITION, //Load lai scene cu nhung van co transition
        RELOAD_WITHOUT_TRANSITION //Load lai scene cu va khong co transition
    }
    
    public enum SceneType
    {
        UNKNOWN = -999,
        MAIN_MENU = 0,
        GAMEPLAY,
    }
    public enum PenguinState
    {
        TO_START,
        WAITING,
        TO_LAND,
        FINISH,
        DEAD,
    }
    public enum LandDirect
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        UP_LEFT,
        UP_RIGHT,
        DOWN_LEFT,
        DOWN_RIGHT,
    }
    public enum Direct
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }
    public enum GameplayState
    {
        START_GAME,
        PLAYING,
        END_GAME,
    }
}
