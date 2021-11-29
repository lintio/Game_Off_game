using System;
public class AntSystem
{
    public event EventHandler OnAntCountChanged;

    private float antCount;
    private int antCountMax;

    private float antsInUse;

    public AntSystem(int ants)
    {
        this.antCountMax = ants;
        this.antCount = antCountMax;
    }

    public float GetAntCount()
    {
        return antCount;
    }

    public float GetAntsInUse()
    {
        return antsInUse;
    }

    public float GetAntCountPercent()
    {
        return antCount / antCountMax;
    }

    public float GetAntsInUsePercent()
    {
        return antsInUse / antCountMax;
    }

    public void SetAntsInUse(float antAmount)
    {
        antCount -= antAmount;
        antsInUse += antAmount;
        if (antCount <= 0)
            antCount = 0;

        OnAntCountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ReleaseAntsFromUse(float antAmount)
    {
        antCount += antAmount;
        antsInUse -= antAmount;
        if (antCount > antCountMax)
            antCount = antCountMax;

        OnAntCountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseAntCountMax(int newAntsCollected)
    {
        antCount += newAntsCollected;
        antCountMax += newAntsCollected;
        OnAntCountChanged?.Invoke(this, EventArgs.Empty);
    }
}
