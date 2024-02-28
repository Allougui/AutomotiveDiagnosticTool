namespace AutomotiveDiagnosticTool.API
{
    public class DiagnosticData
    {
        public int DiagnosticDataId { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
       
        public string DiagnosticType { get; set; }
        public int UserId { get; set; } 

        public User User { get; set; } 

        
       
    }
}
