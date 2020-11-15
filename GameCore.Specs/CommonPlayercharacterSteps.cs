using TechTalk.SpecFlow;

namespace GameCore.Specs
{
    [Binding]
    public class CommonPlayerCharacterSteps
    {
        public CommonPlayerCharacterSteps(PlayerCharacterStepsContext context)
        {
            _context = context;
        }
        //private PlayerCharacter _player;
        private readonly PlayerCharacterStepsContext _context;

        [Given(@"I'm a new player")]
        public void GivenImANewPlayer()
        {
            _context.Player = new PlayerCharacter();
        }
    }
}
