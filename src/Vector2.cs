public class Vector2
{
    public float x;
    public float y;
    public static Vector2 zero = new Vector2(0f, 0f);
    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2(int x, int y)
    {
        this.x = (float)x;
        this.y = (float)y;
    }

    public override bool Equals(object obj)
    {
        var other = obj as Vector2;
        if (other == null) return false;
        return this.x == other.x && this.y == other.y;
    }

    public override int GetHashCode()
    {
        return (int)(this.x * 10000 + this.y * 100);
    }
    
    public override string ToString()
    {
        return "(" + this.x.ToString("G18") + "," + this.y.ToString("G18") + ")";
    }
}