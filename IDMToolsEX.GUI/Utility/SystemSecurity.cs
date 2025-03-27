using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace IDMToolsEX.Utility;

public class SystemSecurity
{
    private static readonly Dictionary<string, string[]> Policies = new()
    {
        { @"Software\Microsoft\Windows\CurrentVersion\Policies\System", ["DisableTaskMgr", "DisableRegistryTools"] },
        { @"Software\Policies\Microsoft\Windows\System", ["DisableCMD"] },
        {
            @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", [
                "DisallowRun", "RestrictRun", "NoKnownFolders", "EnforceShellExtensionSecurity", "NoDrives",
                "NoFileMenu", "NoManageMyComputerVerb", "NoSharedDocuments", "NoViewContextMenu", "NoViewOnDrive",
                "NoWritingToUSB"
            ]
        }
    };

    public bool ArePoliciesEnabled()
    {
        Console.WriteLine("Checking if specified Group Policies are enabled...");

        foreach (var policy in Policies)
        foreach (var valueName in policy.Value)
            if (!IsRegistryValueSet(policy.Key, valueName))
            {
                Console.WriteLine($"Policy {valueName} in {policy.Key} is not enabled.");
                return false;
            }

        Console.WriteLine("All specified Group Policies are enabled.");
        return true;
    }

    public void DisablePolicies()
    {
        Console.WriteLine("Disabling specified Group Policies...");

        foreach (var policy in Policies)
        foreach (var valueName in policy.Value)
            DeleteRegistryValue(policy.Key, valueName);

        Console.WriteLine("Group Policies have been disabled.");
    }

    public void EnablePolicies()
    {
        Console.WriteLine("Enabling specified Group Policies...");

        foreach (var policy in Policies)
        foreach (var valueName in policy.Value)
            SetRegistryValue(policy.Key, valueName, 0);

        Console.WriteLine("Group Policies have been enabled.");
    }

    private static bool IsRegistryValueSet(string path, string valueName)
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(path);
            return key?.GetValue(valueName) != null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking {valueName} in {path}: {ex.Message}");
            return false;
        }
    }

    private static void DeleteRegistryValue(string path, string valueName)
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(path, true);
            if (key?.GetValue(valueName) != null)
            {
                key.DeleteValue(valueName);
                Console.WriteLine($"Deleted: {valueName} from {path}");
            }
            else
            {
                Console.WriteLine($"Not found: {valueName} in {path}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting {valueName} in {path}: {ex.Message}");
        }
    }

    private static void SetRegistryValue(string path, string valueName, int value)
    {
        try
        {
            using var key = Registry.CurrentUser.CreateSubKey(path);
            key.SetValue(valueName, value, RegistryValueKind.DWord);
            Console.WriteLine($"Set: {valueName} = {value} in {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting {valueName} in {path}: {ex.Message}");
        }
    }
}
