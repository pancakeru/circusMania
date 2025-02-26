
public class AnimalControlFox : AbstractSpecialAnimal
{
    private BuffFox selfBuff;
    public override void DoWhenShowStart()
    {
        selfBuff = new BuffFox(animalBody);
        BuffManager.instance.AddGiveExtraBuff(selfBuff);
    }

    public override void ResetWhenBackToInitial()
    {
        base.ResetWhenBackToInitial();
        BuffManager.instance.RemoveGiveExtraBuff(selfBuff);
    }
}
