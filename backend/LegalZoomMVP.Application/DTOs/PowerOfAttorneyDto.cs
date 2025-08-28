// DTO for Power of Attorney (POA) form
namespace LegalZoomMVP.Application.DTOs
{
    public class PowerOfAttorneyDto
    {
        // Section 1: About You
        public string StateOfResidence { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string IsMarried { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        // Section 2: Your Agent
        public string AgentFirstName { get; set; } = string.Empty;
        public string AgentLastName { get; set; } = string.Empty;
        public string AgentCity { get; set; } = string.Empty;
        public string AgentState { get; set; } = string.Empty;
        public string AgentZipCode { get; set; } = string.Empty;
        public string AlternateAgentName { get; set; } = string.Empty;

        // Section 3: Powers
        public bool RealEstate { get; set; }
        public bool PersonalProperty { get; set; }
        public bool Banking { get; set; }
        public bool Stocks { get; set; }
        public bool BusinessOperations { get; set; }
        public bool RetirementPlans { get; set; }
        public bool Insurance { get; set; }
        public bool EstateTrusts { get; set; }
        public bool GovernmentAssistance { get; set; }
        public bool PersonalFamilyCare { get; set; }
        public bool MakingGifts { get; set; }
        public bool PetCare { get; set; }

        // Section 4: Details
        public string EffectiveDate { get; set; } = string.Empty; // "immediately" or "incapacitated"
        public string RevokePrior { get; set; } = string.Empty;
        public string AdditionalInstructions { get; set; } = string.Empty;
    }
}
