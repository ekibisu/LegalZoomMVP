using System;

namespace LegalZoomMVP.Domain.Entities
{
    public class PowerOfAttorney
    {
        public int Id { get; set; }
        // Section 1: About You
        public string StateOfResidence { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string IsMarried { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        // Section 2: Your Agent
        public string AgentFirstName { get; set; }
        public string AgentLastName { get; set; }
        public string AgentCity { get; set; }
        public string AgentState { get; set; }
        public string AgentZipCode { get; set; }
        public string AlternateAgentName { get; set; }

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
        public string EffectiveDate { get; set; }
        public string RevokePrior { get; set; }
        public string AdditionalInstructions { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
