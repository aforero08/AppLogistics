using NonFactors.Mvc.Lookup;
using System;

namespace AppLogistics.Objects;

public class EmployeeView : BaseView<Employee>
{
    [LookupColumn]
    public string InternalCode { get; set; }

    public string DocumentTypeName { get; set; }

    public string DocumentNumber { get; set; }

    public bool Active { get; set; }

    [LookupColumn]
    public string Name { get; set; }

    [LookupColumn]
    public string Surname { get; set; }

    public DateTime BornDate { get; set; }

    public DateTime HireDate { get; set; }

    public DateTime? RetirementDate { get; set; }

    public string ResidenceCity { get; set; }

    public string Address { get; set; }

    public int SocialClass { get; set; }

    public string CountryName { get; set; }

    public string BirthPlace { get; set; }

    public string MobilePhone { get; set; }

    public string HomePhone { get; set; }

    public string Email { get; set; }

    public string MaritalStatusName { get; set; }

    public string SexName { get; set; }

    public string EthnicGroupName { get; set; }

    public string EducationLevelName { get; set; }

    public string EmergencyContact { get; set; }

    public string EmergencyContactPhone { get; set; }

    public string AfpName { get; set; }

    public string EpsName { get; set; }

    public bool HasCurriculumVitae { get; set; }

    public bool HasDocumentCopy { get; set; }

    public bool HasResidencePermit { get; set; }

    public bool HasPhotos { get; set; }

    public bool HasMilitaryIdCopy { get; set; }

    public bool HasLaborCertification { get; set; }

    public bool HasPersonalReference { get; set; }

    public bool HasDisciplinaryBackground { get; set; }

    public bool HasKnowledgeTest { get; set; }

    public bool HasAdmissionTest { get; set; }

    public bool HasContract { get; set; }

    public bool HasInternalRegulations { get; set; }

    public bool HasEndownmentLetter { get; set; }

    public bool TrainingBASC { get; set; }

    public bool TrainingSGSST { get; set; }

    public bool TrainingBPM { get; set; }

    public string TrainingOthers { get; set; }

    public bool IsCriticalPosition { get; set; }

    public string Comments { get; set; }

    public string Section_Documents { get; set; }

    public string Section_GeneralInfo { get; set; }

    public string Section_Trainings { get; set; }
}
