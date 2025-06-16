using System;

public static class AchievementMediator
{
    public static event Action OnFirstHatch;
    public static event Action OnNewCaretaker;
    public static event Action OnBathTime;
    public static event Action OnSnackTime;
    public static event Action OnPlayTime;
    public static event Action OnBoosterShopper;
    public static event Action OnCheater;
    public static event Action OnStylist;
    public static event Action OnGrowTime;
    public static event Action OnSeller;
    public static event Action OnNewEnvironment;
    
    public static void InvokeFirstHatch() => OnFirstHatch?.Invoke();
    public static void InvokeNewCaretaker() => OnNewCaretaker?.Invoke();
    public static void InvokeBathTime() => OnBathTime?.Invoke();
    public static void InvokeSnackTime() => OnSnackTime?.Invoke();
    public static void InvokePlayTime() => OnPlayTime?.Invoke();
    public static void InvokeBoosterShopper() => OnBoosterShopper?.Invoke();
    public static void InvokeCheater() => OnCheater?.Invoke();
    public static void InvokeStylist() => OnStylist?.Invoke();
    public static void InvokeGrowTime() => OnGrowTime?.Invoke();
    public static void InvokeSeller() => OnSeller?.Invoke();
    public static void InvokeNewEnvironment() => OnNewEnvironment?.Invoke();
}
