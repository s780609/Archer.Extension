// See https://aka.ms/new-console-template for more information
using Archer.Extension;
using Archer.Extension.Models;

Console.WriteLine("Archer.Extension.Testing Start...");

// 測試自然人法人判斷功能
Console.WriteLine("\n=== 自然人法人判斷測試 ===");

// 測試身分證號（自然人）
string[] testNationalIds = { "A123456789", "F131232899", "B227549687" };
foreach (var id in testNationalIds)
{
    var entityType = id.GetEntityType();
    var isNatural = id.IsNaturalPerson();
    var isValid = id.IsValidNationalId();
    Console.WriteLine($"身分證號 {id}: {entityType.GetDescription()}, 是自然人: {isNatural}, 格式有效: {isValid}");
}

// 測試統一編號（法人）
string[] testBusinessIds = { "12345678", "53212539", "70759642" };
foreach (var id in testBusinessIds)
{
    var entityType = id.GetEntityType();
    var isLegal = id.IsLegalEntity();
    var isValid = id.IsValidBusinessNumber();
    Console.WriteLine($"統一編號 {id}: {entityType.GetDescription()}, 是法人: {isLegal}, 格式有效: {isValid}");
}

// 測試無效格式
string[] testInvalidIds = { "invalid", "123", "", null };
foreach (var id in testInvalidIds)
{
    var entityType = id.GetEntityType();
    Console.WriteLine($"無效識別號 '{id}': {entityType.GetDescription()}");
}

// 使用IdentityHelper類別的直接方法測試
Console.WriteLine("\n=== 直接使用IdentityHelper測試 ===");
Console.WriteLine($"直接調用 IdentityHelper.DetermineEntityType('A123456789'): {IdentityHelper.DetermineEntityType("A123456789").GetDescription()}");
Console.WriteLine($"直接調用 IdentityHelper.IsValidNationalId('A123456789'): {IdentityHelper.IsValidNationalId("A123456789")}");
Console.WriteLine($"直接調用 IdentityHelper.IsValidBusinessRegistrationNumber('12345678'): {IdentityHelper.IsValidBusinessRegistrationNumber("12345678")}");

Console.WriteLine("\n測試完成！按任意鍵退出...");
Console.ReadLine();

public class Loan
{
    public string LoanID { get; set; } = null;

    public string LoanType { get; set; } = null;

    public string ApplicationDate { get; set; } = null;

    public string NationalId { get; set; } = null;

    public string SeqNo { get; set; } = null;

    public DateTime? ApplicationDateTime { get; set; } = null;

    public string BirthYear { get; set; } = null;

    public string BirthMonth { get; set; } = null;

    public string BirthDay { get; set; } = null;

    public string CellPhone { get; set; } = null;

    public string Name { get; set; } = null;

    public string MaritalStatus { get; set; } = null;

    public string Education { get; set; } = null;

    public bool? IsEstate { get; set; } = null;

    public string ContactPhoneArea { get; set; } = null;

    public string ContactPhoneNum { get; set; } = null;

    public string ContactPhoneExt { get; set; } = null;

    public string Email { get; set; } = null;

    public bool? IsOtherCitizenship { get; set; } = null;

    public string OtherCountry { get; set; } = null;

    public string OtherCity { get; set; } = null;

    public bool? IsOtherTaxpayer { get; set; } = null;

    public string CompanyName { get; set; } = null;

    public string TaxIdNum { get; set; } = null;

    public string CompanyPhoneArea { get; set; } = null;

    public string CompanyPhoneNum { get; set; } = null;

    public string CompanyPhoneExt { get; set; } = null;

    public string JobPosition { get; set; } = null;

    public string JobTitle { get; set; } = null;

    public string Seniority { get; set; } = null;

    public int? AnnualIncome { get; set; } = null;

    public int? LoanAmount { get; set; } = null;

    public int? LoanPeriod { get; set; } = null;

    public string LoanPurpose { get; set; } = null;

    public string ImportBank { get; set; } = null;

    public string ImportBranch { get; set; } = null;

    public string ImportAccount { get; set; } = null;

    public bool? IsGuaranteed { get; set; } = null;

    public string PromotionId { get; set; } = null;

    public string Unit { get; set; } = null;

    public string IdentifyMethod { get; set; } = null;

    public string RemoteIP { get; set; } = null;

    public string Status { get; set; } = null;

    public string ReplyCaseType { get; set; } = null;

    public bool? IsDigital { get; set; } = null;

    public string RiskLevel { get; set; } = null;

    public string RiskLevelTime { get; set; } = null;

    public string PromotionName { get; set; } = null;

    public bool? IsStakeholder { get; set; } = null;

    public string AreaUsed { get; set; } = null;

    public bool? IsExceedRelativeLimit { get; set; } = null;

    public bool? IsRule72 { get; set; } = null;

    public string PurposeType { get; set; } = null;

    public string MonthlyIncome { get; set; } = null;

    public string CreditDescription { get; set; } = null;

    public string Instruction { get; set; } = null;

    public string UsingWay { get; set; } = null;

    public string ProjectCode { get; set; } = null;

    public string GroupCode { get; set; } = null;

    public string OtherOpinions { get; set; } = null;

    public string Permission { get; set; } = null;

    public string Note { get; set; } = null;

    public string NoteObject { get; set; } = null;

    public DateTime? NoteTime { get; set; } = null;

    public string Note2 { get; set; } = null;

    public string NoteObject2 { get; set; } = null;

    public DateTime? NoteTime2 { get; set; } = null;

    public string Manager { get; set; } = null;

    public DateTime? ManageTime { get; set; } = null;

    public string ReviewSupervisor { get; set; } = null;

    public DateTime? ReviewTime { get; set; } = null;

    public string ApproveSupervisor { get; set; } = null;

    public DateTime? ApproveTime { get; set; } = null;

    public string WithdrawSupervisor { get; set; } = null;

    public string WithdrawReason { get; set; } = null;

    public string TransferToBranch { get; set; } = null;

    public string TransferReason { get; set; } = null;

    public string Transfer { get; set; } = null;

    public bool? IsNoteMatch { get; set; } = null;

    public DateTime? ConditionConfirmTime { get; set; } = null;

    public DateTime? ModifyManagerTime { get; set; } = null;

    public string Supervisor3 { get; set; } = null;

    public DateTime? SupervisorTime3 { get; set; } = null;

    public string Supervisor4 { get; set; } = null;

    public DateTime? SupervisorTime4 { get; set; } = null;

    public string ModifyReason { get; set; } = null;

    public DateTime? ConditionModifyTime { get; set; } = null;

    public DateTime? SignUpTime { get; set; } = null;

    public string Grantor { get; set; } = null;

    public DateTime? GrantTime { get; set; } = null;

    public string DebitAccountType { get; set; } = null;

    public string DebitAccount { get; set; } = null;

    public string BusinessRelationship { get; set; } = null;

    public string CertifiedDoc { get; set; } = null;

    public string BusinessPurpose { get; set; } = null;

    public bool? IsOBU { get; set; } = null;

    public string NaturalPerson { get; set; } = null;

    public DateTime? ModifySupervisorTime { get; set; } = null;

    public bool? IsModified { get; set; } = null;

    public string ApplyDigitalAccountState { get; set; } = null;

    public string StatusReason { get; set; } = null;

    public DateTime? ExpiredTime { get; set; } = null;

    public string SignUpIP { get; set; } = null;

    public string FeeDiscount { get; set; } = null;

    public string Nationality { get; set; } = null;

    public string BirthPlace { get; set; } = null;

    public string UploadIP { get; set; } = null;

    public DateTime? UploadTime { get; set; } = null;

    public int? IncomeRatio { get; set; } = null;

    public string IncomeRatioInfo { get; set; } = null;

    public string UbotApplicationIpInfo { get; set; } = null;

    public int? ReplenishmentNoticeCount { get; set; } = null;

    public DateTime? ReplenishmentNoticeTime { get; set; } = null;

    public string StatusReasonInfo { get; set; } = null;

    public byte? Department { get; set; } = null;

    public byte? CollateralType { get; set; } = null;

    public string OverviewBusinessType { get; set; } = null;

    public string KycJobId { get; set; } = null;

    public string KycJobDescription { get; set; } = null;

    public string ModifyManager { get; set; } = null;

    public string ModifySupervisor { get; set; } = null;

    public bool? IsApp { get; set; }
}

public enum Test
{
    TopLeft,
    TopCenter,
    TopRight,
    MiddleLeft,
    MiddleCenter,
    MiddleRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}