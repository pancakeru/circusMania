
public class AnimalControlFox : AbstractSpecialAnimal
{
    private BuffFox selfBuff;
    public override void DoWhenShowStart()
    {
        selfBuff = new BuffFox(animalBody, soul.skillNum);
        BuffManager.instance.AddGiveExtraBuff(selfBuff);
    }

    public override void ResetWhenBackToInitial()
    {
        base.ResetWhenBackToInitial();
        BuffManager.instance.RemoveGiveExtraBuff(selfBuff);
    }
}
