using app.common.Utils.Enums;

namespace app.common.DTO
{
    public class StateValue
    {
        public ActiveBlockType activeBlockType { get; set; }
        public StateQuestionProf stateQuestion {get;set;}
        public StateFaculty stateFaculty { get; set; }
    }
}