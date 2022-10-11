namespace Dino.Units.Component
{
    public interface IFieldOfViewRenderer
    {
        void Init(float angle, float radius);
        void SetActive(bool value);
    }
}