namespace Epsilon.Abstractions.Model;

public class HboIDomain2018 : IHboIDomain
{
    public static readonly ArchitectureLayer HardwareInterfacing = new(0, "Hardware Interfacing", "H", "#8D9292");
    public static readonly ArchitectureLayer Infrastructure = new(1, "Infrastructure", "I", "#6EA7D4");
    public static readonly ArchitectureLayer OrganisationalProcesses = new(2, "Organisational Processes", "O", "#D16557");
    public static readonly ArchitectureLayer UserInteraction = new(3, "User Interaction", "U", "#E29C53");
    public static readonly ArchitectureLayer Software = new(4, "Software", "S", "#96B9C0");

    public static readonly Activity ManageAndControl = new(0, "Manage & Control");
    public static readonly Activity Analysis = new(1, "Analysis");
    public static readonly Activity Advise = new(2, "Advise");
    public static readonly Activity Design = new(3, "Design");
    public static readonly Activity Realisation = new(4, "Realisation");

    public static readonly MasteryLevel LevelOne = new(0, 1, "#00B0F0");
    public static readonly MasteryLevel LevelTwo = new(1, 2, "#00B050");
    public static readonly MasteryLevel LevelThree = new(2, 3, "#FFFC00");
    public static readonly MasteryLevel LevelFour = new(3, 4);

    public static readonly ProfessionalSkill FutureOrientedOrganisation = new(0, "Future-Oriented Organisation", "FOO");
    public static readonly ProfessionalSkill InvestigativeProblemSolving = new(1, "Investigative Problem Solving", "IPS");
    public static readonly ProfessionalSkill PersonalLeadership = new(2, "Personal Leadership", "PL");
    public static readonly ProfessionalSkill TargetedInteraction = new(3, "Targeted Interaction", "TI");

    public IEnumerable<ArchitectureLayer> ArchitectureLayers => new[]
    {
        HardwareInterfacing,
        Infrastructure,
        OrganisationalProcesses,
        UserInteraction,
        Software,
    };

    public IEnumerable<Activity> Activities => new[]
    {
        ManageAndControl,
        Analysis,
        Advise,
        Design,
        Realisation,
    };

    public IEnumerable<ProfessionalSkill> ProfessionalSkills => new[]
    {
        FutureOrientedOrganisation,
        InvestigativeProblemSolving,
        PersonalLeadership,
        TargetedInteraction,
    };

    public IEnumerable<MasteryLevel> MasteryLevels => new[]
    {
        LevelOne,
        LevelTwo,
        LevelThree,
        LevelFour,
    };
}