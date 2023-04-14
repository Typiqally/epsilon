using Epsilon.Abstractions.Model;

namespace Epsilon;

public static class FhictConstants
{
    public static readonly IDictionary<int, ProfessionalTask> ProfessionalTasks = new Dictionary<int, ProfessionalTask>
    {
        { 6785, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelOne.Id) },
        { 6786, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelOne.Id) },
        { 6787, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6788, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6789, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelThree.Id) },
        { 6790, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelFour.Id) },
        { 6791, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelFour.Id) },
        { 6792, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelFour.Id) },
        { 6773, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelOne.Id) },
        { 6774, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelOne.Id) },
        { 6775, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelOne.Id) },
        { 6776, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6777, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6778, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6779, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6780, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6781, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6782, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6783, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelFour.Id) },
        { 6784, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelFour.Id) },
        { 6801, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelOne.Id) },
        { 6802, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6803, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelThree.Id) },
        { 6804, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelFour.Id) },
        { 6812, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelOne.Id) },
        { 6813, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelTwo.Id) },
        { 6814, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelTwo.Id) },
        { 6815, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelThree.Id) },
        { 6816, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelFour.Id) },
        { 6805, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelOne.Id) },
        { 6806, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6807, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6808, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6809, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelThree.Id) },
        { 6810, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelThree.Id) },
        { 6811, new ProfessionalTask(HboIDomain2018.HardwareInterfacing.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelFour.Id) },
        { 6863, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelOne.Id) },
        { 6864, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6865, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6866, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelThree.Id) },
        { 6867, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelThree.Id) },
        { 6868, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelFour.Id) },
        { 6857, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelOne.Id) },
        { 6858, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6859, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6860, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6861, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6862, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelFour.Id) },
        { 6869, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelOne.Id) },
        { 6870, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6871, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6872, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6873, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelThree.Id) },
        { 6874, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelThree.Id) },
        { 6875, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelFour.Id) },
        { 6885, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelOne.Id) },
        { 6886, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelTwo.Id) },
        { 6887, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelTwo.Id) },
        { 6888, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelTwo.Id) },
        { 6889, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelThree.Id) },
        { 6890, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelThree.Id) },
        { 6891, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelFour.Id) },
        { 6876, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelOne.Id) },
        { 6877, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6878, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6879, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6880, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelThree.Id) },
        { 6881, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelThree.Id) },
        { 6882, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelThree.Id) },
        { 6883, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelFour.Id) },
        { 6884, new ProfessionalTask(HboIDomain2018.Infrastructure.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelFour.Id) },
        { 6903, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelOne.Id) },
        { 6904, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6905, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6906, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelThree.Id) },
        { 6907, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelThree.Id) },
        { 6908, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelThree.Id) },
        { 6909, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelFour.Id) },
        { 6910, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelFour.Id) },
        { 6892, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelOne.Id) },
        { 6893, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelOne.Id) },
        { 6894, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6895, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6896, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6897, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6898, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6899, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6900, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6901, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6902, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelFour.Id) },
        { 6911, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelOne.Id) },
        { 6912, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6913, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6914, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6915, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelThree.Id) },
        { 6916, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelThree.Id) },
        { 6917, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelThree.Id) },
        { 6918, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelFour.Id) },
        { 6919, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelFour.Id) },
        { 6930, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelOne.Id) },
        { 6931, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelOne.Id) },
        { 6932, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelTwo.Id) },
        { 6933, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelTwo.Id) },
        { 6934, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelThree.Id) },
        { 6935, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelThree.Id) },
        { 6920, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelOne.Id) },
        { 6921, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelOne.Id) },
        { 6922, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelOne.Id) },
        { 6923, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6924, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6925, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6926, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6927, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelThree.Id) },
        { 6928, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelThree.Id) },
        { 6929, new ProfessionalTask(HboIDomain2018.OrganisationalProcesses.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelFour.Id) },
        { 6825, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelOne.Id) },
        { 6826, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6827, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6828, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6829, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelThree.Id) },
        { 6830, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelThree.Id) },
        { 6831, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelThree.Id) },
        { 6832, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelFour.Id) },
        { 6817, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelOne.Id) },
        { 6818, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelOne.Id) },
        { 6819, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6820, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6821, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6822, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6823, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6824, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelFour.Id) },
        { 6833, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelOne.Id) },
        { 6834, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6835, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6836, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6837, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6838, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6839, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelThree.Id) },
        { 6840, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelThree.Id) },
        { 6841, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelFour.Id) },
        { 6842, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelFour.Id) },
        { 6851, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelOne.Id) },
        { 6852, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelTwo.Id) },
        { 6853, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelTwo.Id) },
        { 6854, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelThree.Id) },
        { 6855, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelThree.Id) },
        { 6856, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelFour.Id) },
        { 6843, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelOne.Id) },
        { 6844, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6845, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6846, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6847, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelThree.Id) },
        { 6848, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelThree.Id) },
        { 6849, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelFour.Id) },
        { 6850, new ProfessionalTask(HboIDomain2018.Software.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelFour.Id) },
        { 6946, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelOne.Id) },
        { 6947, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelOne.Id) },
        { 6948, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6949, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6950, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelTwo.Id) },
        { 6951, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelThree.Id) },
        { 6952, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelThree.Id) },
        { 6953, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Advise.Id, HboIDomain2018.LevelFour.Id) },
        { 6936, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelOne.Id) },
        { 6937, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelOne.Id) },
        { 6938, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelOne.Id) },
        { 6939, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6940, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6941, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelTwo.Id) },
        { 6942, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6943, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6944, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelThree.Id) },
        { 6945, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Analysis.Id, HboIDomain2018.LevelFour.Id) },
        { 6954, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelOne.Id) },
        { 6955, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelOne.Id) },
        { 6956, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelOne.Id) },
        { 6957, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6958, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelTwo.Id) },
        { 6959, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelThree.Id) },
        { 6960, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelThree.Id) },
        { 6961, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Design.Id, HboIDomain2018.LevelFour.Id) },
        { 6970, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelOne.Id) },
        { 6971, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelTwo.Id) },
        { 6972, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelTwo.Id) },
        { 6973, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelThree.Id) },
        { 6974, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelThree.Id) },
        { 6975, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelThree.Id) },
        { 6976, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.ManageAndControl.Id, HboIDomain2018.LevelFour.Id) },
        { 6962, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelOne.Id) },
        { 6963, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6964, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6965, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 6966, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelThree.Id) },
        { 6967, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelThree.Id) },
        { 6968, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelFour.Id) },
        { 6969, new ProfessionalTask(HboIDomain2018.UserInteraction.Id, HboIDomain2018.Realisation.Id, HboIDomain2018.LevelFour.Id) },
    };

    public static readonly IDictionary<int, ProfessionalSkillLevel> ProfessionalSkills = new Dictionary<int, ProfessionalSkillLevel>
    {
        { 22323, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelOne.Id) },
        { 22324, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelOne.Id) },
        { 22338, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelOne.Id) },
        { 22339, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelOne.Id) },
        { 22340, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelOne.Id) },
        { 22325, new ProfessionalSkillLevel(HboIDomain2018.InvestigativeProblemSolving.Id, HboIDomain2018.LevelOne.Id) },
        { 22326, new ProfessionalSkillLevel(HboIDomain2018.InvestigativeProblemSolving.Id, HboIDomain2018.LevelOne.Id) },
        { 22327, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelOne.Id) },
        { 22328, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelOne.Id) },
        { 22329, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelOne.Id) },
        { 22330, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelOne.Id) },
        { 22331, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelOne.Id) },
        { 22332, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelOne.Id) },
        { 22333, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelOne.Id) },
        { 22334, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelOne.Id) },
        { 22335, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelOne.Id) },
        { 22336, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelOne.Id) },
        { 22337, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelOne.Id) },
        { 11460, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 11462, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 11463, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 11464, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 11465, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelTwo.Id) },
        { 11466, new ProfessionalSkillLevel(HboIDomain2018.InvestigativeProblemSolving.Id, HboIDomain2018.LevelTwo.Id) },
        { 11467, new ProfessionalSkillLevel(HboIDomain2018.InvestigativeProblemSolving.Id, HboIDomain2018.LevelTwo.Id) },
        { 11468, new ProfessionalSkillLevel(HboIDomain2018.InvestigativeProblemSolving.Id, HboIDomain2018.LevelTwo.Id) },
        { 11469, new ProfessionalSkillLevel(HboIDomain2018.InvestigativeProblemSolving.Id, HboIDomain2018.LevelTwo.Id) },
        { 11470, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelTwo.Id) },
        { 11471, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelTwo.Id) },
        { 11472, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelTwo.Id) },
        { 11473, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelTwo.Id) },
        { 11474, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelTwo.Id) },
        { 11475, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelTwo.Id) },
        { 11476, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelTwo.Id) },
        { 11477, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelTwo.Id) },
        { 11478, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelTwo.Id) },
        { 11479, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelTwo.Id) },
        { 11480, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelTwo.Id) },
        { 11481, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelTwo.Id) },
        { 12286, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelThree.Id) },
        { 12287, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelThree.Id) },
        { 12288, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelThree.Id) },
        { 12289, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelThree.Id) },
        { 12290, new ProfessionalSkillLevel(HboIDomain2018.FutureOrientedOrganisation.Id, HboIDomain2018.LevelThree.Id) },
        { 12291, new ProfessionalSkillLevel(HboIDomain2018.InvestigativeProblemSolving.Id, HboIDomain2018.LevelThree.Id) },
        { 12292, new ProfessionalSkillLevel(HboIDomain2018.InvestigativeProblemSolving.Id, HboIDomain2018.LevelThree.Id) },
        { 12293, new ProfessionalSkillLevel(HboIDomain2018.InvestigativeProblemSolving.Id, HboIDomain2018.LevelThree.Id) },
        { 12294, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelThree.Id) },
        { 12295, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelThree.Id) },
        { 12296, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelThree.Id) },
        { 12297, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelThree.Id) },
        { 12298, new ProfessionalSkillLevel(HboIDomain2018.PersonalLeadership.Id, HboIDomain2018.LevelThree.Id) },
        { 12299, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelThree.Id) },
        { 12300, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelThree.Id) },
        { 12301, new ProfessionalSkillLevel(HboIDomain2018.TargetedInteraction.Id, HboIDomain2018.LevelThree.Id) },
    };
}