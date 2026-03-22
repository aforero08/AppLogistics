using System;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class EmployeeCreateEditView : BaseView<Employee>
{
    [Required]
    [StringLength(16)]
    public string InternalCode { get; set; }

    [Required]
    public int DocumentTypeId { get; set; }

    [Required]
    [StringLength(16)]
    public string DocumentNumber { get; set; }

    [Required]
    public bool Active { get; set; }

    [Required]
    [StringLength(32)]
    public string Name { get; set; }

    [Required]
    [StringLength(32)]
    public string Surname { get; set; }

    [Required]
    public DateTime BornDate { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    public DateTime? RetirementDate { get; set; }

    [Required]
    [StringLength(32)]
    public string ResidenceCity { get; set; }

    [Required]
    [StringLength(128)]
    public string Address { get; set; }

    [Required]
    [Range(1, 6)]
    public int SocialClass { get; set; }

    [Required]
    public int CountryId { get; set; }

    [Required]
    [StringLength(128)]
    public string BirthPlace { get; set; }

    [Phone]
    [Required]
    [StringLength(16)]
    public string MobilePhone { get; set; }

    [Phone]
    [StringLength(16)]
    public string HomePhone { get; set; }

    [EmailAddress]
    [StringLength(128)]
    public string Email { get; set; }

    [Required]
    public int MaritalStatusId { get; set; }

    [Required]
    public int SexId { get; set; }

    [Required]
    public int EthnicGroupId { get; set; }

    [Required]
    public int EducationLevelId { get; set; }

    [StringLength(32)]
    public string EmergencyContact { get; set; }

    [Phone]
    [StringLength(16)]
    public string EmergencyContactPhone { get; set; }

    [Required]
    public int AfpId { get; set; }

    [Required]
    public int EpsId { get; set; }

    [Required]
    public bool HasCurriculumVitae { get; set; }

    [Required]
    public bool HasDocumentCopy { get; set; }

    [Required]
    public bool HasResidencePermit { get; set; }

    [Required]
    public bool HasPhotos { get; set; }

    [Required]
    public bool HasMilitaryIdCopy { get; set; }

    [Required]
    public bool HasLaborCertification { get; set; }

    [Required]
    public bool HasPersonalReference { get; set; }

    [Required]
    public bool HasDisciplinaryBackground { get; set; }

    [Required]
    public bool HasKnowledgeTest { get; set; }

    [Required]
    public bool HasAdmissionTest { get; set; }

    [Required]
    public bool HasContract { get; set; }

    [Required]
    public bool HasInternalRegulations { get; set; }

    [Required]
    public bool HasEndownmentLetter { get; set; }

    [Required]
    public bool TrainingBASC { get; set; }

    [Required]
    public bool TrainingSGSST { get; set; }

    [Required]
    public bool TrainingBPM { get; set; }

    [StringLength(512)]
    public string TrainingOthers { get; set; }

    [Required]
    public bool IsCriticalPosition { get; set; }

    [StringLength(512)]
    public string Comments { get; set; }

    public string Section_Documents { get; set; }

    public string Section_GeneralInfo { get; set; }

    public string Section_Trainings { get; set; }
}
