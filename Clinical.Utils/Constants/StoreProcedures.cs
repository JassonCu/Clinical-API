namespace Clinical.Utils.Constants
{
    public class StoreProcedures
    {
        #region Analysis Store Procedures
        public const string uspAnalysisChangeState = "uspAnalysisChangeState";
        public const string uspAnalysisRegister = "uspAnalysisRegister";
        public const string uspAnalysisRemove = "uspAnalysisRemove";
        public const string uspAnalysisEdit = "uspAnalysisEdit";
        public const string uspAnalysisList = "uspAnalysisList";
        public const string uspAnalysisById = "uspAnalysisById";
        #endregion

        #region Exam Store Procedures
        public const string uspExamList = "uspExamList";
        public const string uspExamById = "uspExamById";
        public const string uspExamRegister = "uspExamRegister";
        #endregion
    }
}