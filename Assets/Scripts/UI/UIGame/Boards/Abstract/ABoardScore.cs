public abstract class ABoardScore : ABoard
{
    protected DataGame _dataGame;

    protected virtual void Awake() => _dataGame = DataGame.InstanceF;

    protected override void TextDefault() => _textBoard.text = "0";
    protected override void ToText(int value) => _textBoard.text = value.ToString();

}
