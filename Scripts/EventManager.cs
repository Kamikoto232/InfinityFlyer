using UnityEngine;

public static class EventManager
{
    //Game
    public delegate void StopGame();
    public delegate void PostStopGame();
    public delegate void StartGame();
    public delegate void PostStartGame();
    public delegate void PauseGame(bool pause);
    public delegate void StartFriction();
    public delegate void Friction();
    public delegate void EndFriction();
    public delegate void CollisionTrash();
    public delegate void Damage(float dmg);
    public delegate void Dead();
    public delegate void ChangeColor(Color color);
    public delegate void ChangeScore(string scrore);
    public delegate void Notification(NotificationData notification);
    public delegate void NearFly();
    public delegate void PickUp(Collectable collectable);
    public delegate void GetHealth(int value);
    public delegate void Boost();

    //Stats
    public delegate void LevelUp(int level);
    public delegate void AddExp(int exp);

    //Visual
    public delegate void MusicPulse(float value);

    //Ships
    public delegate void ChangeShip(ShipData ship);
    public delegate void GetShip(ShipData ship);
    public delegate void UpgradeShip(ShipData ship);

    //UI
    public delegate void OpenSection(Section section);
    public delegate void CloseSection(Section section);
    public delegate void HideSection(Section section);
    public delegate void ShowSection(Section section);

    //Moneys
    public delegate bool Buy((int Credits, int Tokens) Price);
    public delegate void AddMoney((int Credits, int Tokens) Price);

    //Game
    public static event StopGame StopGameHandler = () => { };
    public static event PostStopGame PostStopGameHandler = () => {  };
    public static event StartGame StartGameHandler = () => {  };
    public static event PostStartGame PostStartGameHandler = () => { };
    public static event PauseGame PauseGameHandler = (p) => {  };
    public static event StartFriction StartFrictionHandler = () => { };
    public static event Friction FrictionHandler = () => { };
    public static event EndFriction EndFrictionHandler = () => { };
    public static event CollisionTrash CollisionTrashHandler = () => { };
    public static event Damage DamageHandler = (d) => { };
    public static event Dead DeadHandler = () => { Debug.Log("dead " + DeadHandler.Method.Name);  };
    public static event ChangeColor ChangeColorHandler = (c) => { };
    public static event ChangeScore ChangeScoreHandler = (s) => { };
    public static event Notification NotificationHandler = (n) => { };
    public static event NearFly NearFlyHandler = () => { };
    public static event PickUp PickUpHandler = (c) => { };
    public static event GetHealth GetHealthHandler = (v) => { };
    public static event Boost BoostHandler = () => { };

    //Stats
    public static event LevelUp LevelUpHandler = (l) => { };
    public static event AddExp AddExpHandler = (e) => { };

    //Visual
    public static event MusicPulse MusicPulseHandler = (v) => { };


    //Ships
    public static event ChangeShip ChangeShipHandler = (s) => { };
    public static event GetShip GetShipHandler = (s) => { };
    public static event UpgradeShip UpgradeShipHandler = (s) => { };

    //UI
    public static event OpenSection OpenSectionHandler = (s) => { };
    public static event CloseSection CloseSectionHandler = (s) => { };
    public static event HideSection HideSectionHandler = (s) => { };
    public static event ShowSection ShowSectionHandler = (s) => { };

    //Moneys
    public static event Buy BuyHandler;
    public static event AddMoney AddMoneyHandler;

    //Game
    public static void OnStartGame() => StartGameHandler();
    public static void OnPostStartGame() => PostStartGameHandler();
    public static void OnStopGame() => StopGameHandler();
    public static void OnPostStopGame() => PostStopGameHandler();
    public static void OnPauseGame(bool pause) => PauseGameHandler(pause);
    public static void OnStartFriction() => StartFrictionHandler();
    public static void OnFriction() => FrictionHandler();
    public static void OnEndFriction() => EndFrictionHandler();
    public static void OnCollisionTrash() => CollisionTrashHandler();
    public static void OnDamage(float dmg) => DamageHandler(dmg);
    public static void OnDead() => DeadHandler();
    public static void OnChangeColor(Color color) => ChangeColorHandler(color);
    public static void OnChangeScore(string score) => ChangeScoreHandler(score);
    public static void OnNotification(NotificationData notification) => NotificationHandler(notification);
    public static void OnNearFly() => NearFlyHandler();
    public static void OnPickUp(Collectable collectable) => PickUpHandler(collectable);
    public static void OnGetHealth(int value) => GetHealthHandler(value);
    public static void OnBoost(int value) => BoostHandler();

    //Stats
    public static void OnLevelUp(int lvl) => LevelUpHandler(lvl);
    public static void OnAddExp(int exp) => AddExpHandler(exp);

    //Visual
    public static void OnMusicPulse(float value) => MusicPulseHandler(value);

    //Ships
    public static void OnChangeShip(ShipData ship) => ChangeShipHandler(ship);
    public static void OnGetShip(ShipData ship) => GetShipHandler(ship);
    public static void OnUpgradeShip(ShipData ship) => UpgradeShipHandler(ship);

    //UI
    public static void OnOpenSection(Section section) => OpenSectionHandler(section);
    public static void OnCloseSection(Section section) => CloseSectionHandler(section);
    public static void OnHideSection(Section section) => HideSectionHandler(section);
    public static void OnShowSection(Section section) => ShowSectionHandler(section);

    //Moneys
    public static bool OnBuy((int Credits, int Tokens) Price) => BuyHandler(Price);
    public static void OnAddMoney((int Credits, int Tokens) Price) => AddMoneyHandler(Price);
}