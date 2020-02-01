using UnityEngine;
using System.Collections;


public enum E_HardwareType
{
    iPhone3G = 0,
    iPhone4G = 1,
    iPad = 2,
    Max = 3,
}

public enum E_WeaponType 
{
	None = -1,
	Katana = 0,
	Body,
    Bow,
	Max,
}

public enum E_BlockState
{
    None = -1,
    Start = 0,
    Loop,
    End,
    HitBlocked,
    Failed
}

public enum E_KnockdownState
{
    None = -1,
    Down = 0,
    Loop,
    Up,
    Fatality,
}

public enum E_WeaponState
{
    NotInHands,
	Ready,
	Attacking,
	Reloading,
	Empty,
}

public enum E_AttackType
{
	None = -1,
	X = 0,
	O = 1,
    BossBash = 2,
    Fatality = 3,
    Counter = 4,
    Berserk = 5,
	Max = 6,
}
    
public enum E_EnemyType
{
	None = -1,
    SwordsMan = 0,
    Peasant = 1,
    TwoSwordsMan = 2,
    Bowman = 3,
    PeasantLow = 4,
    MiniBoss01 = 5,
    SwordsManLow = 6,
    notUsed03 = 7,
    notUsed04 = 8,
    notUsed05 = 9,
    BossOrochi = 10,
	Max
}

public enum E_GameState
{
	MainMenu,
	IngameMenu,
	Game,
	SaleScreen,
    Tutorial,
    Shop,
}

public enum E_GameType
{
	SinglePlayer,
    ChapterOnly,
	Survival,
    FirstTimeTutorial,
    Tutorial,
    SaleScreen,
}

public enum E_GameDifficulty
{
    Easy,
	Normal,
	Hard,
}

public enum E_DamageType
{
    Front,
    Back,
    BreakBlock,
    InKnockdown,
    Enviroment,
}

public enum E_CriticalHitType
{
    None,
    Vertical,
    Horizontal,
}

public enum E_DeadBodyType
{
    None = -1,
    Legs = 0,
    Beheaded,
    HalfBody,
    SliceFrontBack,
    SliceLeftRight,
    Max,
}

public enum E_ComboLevel
{
	One = 1,
	Two = 2,
    Three = 3,
    Max = 3
}

public enum E_ComboLevelPrice
{
    One = 0,
    Two = 1000,
    Three = 2000
}

public enum E_SwordLevel
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Max = 5
}

public enum E_SwordLevelPrice
{
    One = 0,
    Two = 1000,
    Three = 1500,
    Four = 2000,
    Five = 3000,
}

public enum E_HealthLevel
{
    One = 1,
    Two = 2,
    Three = 3,
    Max = 3
}

public enum E_HealtLevelPrice
{
    One = 0,
    Two = 1500,
    Three = 3000,
}

public enum E_RotationType
{
    Left,
    Right
}

public enum E_MotionType
{
	None,
	Walk,
	Run,
    Sprint,
    Roll,
    Attack,
    Block,
    BlockingAttack,
    Injury,
    Knockdown,
    Death,
    AnimationDrive,
}

public enum E_MoveType
{
    None,
    Forward,
    Backward,
    StrafeLeft,
    StrafeRight,
}

public enum E_LookType
{
    None,
    TrackTarget,
}


public enum E_InteractionObjects
{
    None,
    UseLever,
    Trigger,
    UseExperience,
    TriggerAnim,
}

public enum E_InteractionType
{
    None,
    On,
    Off
}

public enum E_EventTypes
{
    None,
    EnemyStep,
    EnemySee,
    EnemyLost,
    Hit,
    Died,
    ImInPain,
    HitBlocked,
    Knockdown,
    FriendInjured,
}

public enum E_Direction
{
    Forward,
    Backward,
    Left,
    Right,
    Up,
    Down
}

class Types
{

}