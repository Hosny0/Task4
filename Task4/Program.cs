using System;
using System.Collections.Generic;

public class Account
{
    public string Name { get; set; }
    public double Balance { get; set; }

    public Account(string name = "Unnamed Account", double balance = 0.0)
    {
        this.Name = name;
        this.Balance = balance;
    }

    public virtual bool Deposit(double amount)
    {
        if (amount < 0) return false;
        Balance += amount;
        return true;
    }

    public virtual bool Withdraw(double amount)
    {
        if (Balance - amount >= 0)
        {
            Balance -= amount;
            return true;
        }
        return false;
    }

    public override string ToString() => $"[Account: {Name}, Balance: {Balance}]";
}

public class SavingsAccount : Account
{
    public double InterestRate { get; set; }

    public SavingsAccount(string name = "Unnamed Savings", double balance = 0.0, double interestRate = 0.0)
        : base(name, balance)
    {
        InterestRate = interestRate;
    }

    public override bool Deposit(double amount)
    {
        if (base.Deposit(amount))
        {
            Balance += amount * InterestRate / 100;
            return true;
        }
        return false;
    }

    public override string ToString() =>
        $"[SavingsAccount: {Name}, Balance: {Balance}, Interest: {InterestRate}%]";
}

public class CheckingAccount : Account
{
    private static double Fee = 1.50;

    public CheckingAccount(string name = "Unnamed Checking", double balance = 0.0)
        : base(name, balance) { }

    public override bool Withdraw(double amount)
    {
        return base.Withdraw(amount + Fee);
    }

    public override string ToString() =>
        $"[CheckingAccount: {Name}, Balance: {Balance}]";
}

public class TrustAccount : Account
{
    public double InterestRate { get; set; }
    private int withdrawalCount = 0;
    private const int MaxWithdrawals = 3;
    private const double MaxPercent = 0.2;

    public TrustAccount(string name = "Unnamed Trust", double balance = 0.0, double interestRate = 0.0)
        : base(name, balance)
    {
        InterestRate = interestRate;
    }

    public override bool Deposit(double amount)
    {
        if (amount >= 5000)
            Balance += 50;
        return base.Deposit(amount);
    }

    public override bool Withdraw(double amount)
    {
        if (withdrawalCount >= MaxWithdrawals || amount > Balance * MaxPercent)
            return false;

        if (base.Withdraw(amount))
        {
            withdrawalCount++;
            return true;
        }
        return false;
    }

    public override string ToString() =>
        $"[TrustAccount: {Name}, Balance: {Balance}, Interest: {InterestRate}%, Withdrawals: {withdrawalCount}]";
}

public static class AccountUtil
{
    public static void Deposit(List<Account> accounts, double amount)
    {
        Console.WriteLine("\n Depositing to Accounts ");
        foreach (var acc in accounts)
        {
            if (acc.Deposit(amount))
                Console.WriteLine($"Deposited {amount} to {acc}");
            else
                Console.WriteLine($"Failed to deposit {amount} to {acc}");
        }
    }

    public static void Withdraw(List<Account> accounts, double amount)
    {
        Console.WriteLine("\n Withdrawing from Accounts ");
        foreach (var acc in accounts)
        {
            if (acc.Withdraw(amount))
                Console.WriteLine($"Withdrew {amount} from {acc}");
            else
                Console.WriteLine($"Failed to withdraw {amount} from {acc}");
        }
    }

    public static void Display(List<Account> accounts)
    {
        Console.WriteLine("\n Displaying Accounts ");
        foreach (var acc in accounts)
            Console.WriteLine(acc);
    }
}

class Program
{
    static void Main()
    {
        var accounts = new List<Account>
        {
            new Account(),
            new Account("Larry"),
            new Account("Moe", 2000),
            new Account("Curly", 5000)
        };

        var savAccounts = new List<Account>
        {
            new SavingsAccount(),
            new SavingsAccount("Superman"),
            new SavingsAccount("Batman", 2000),
            new SavingsAccount("Wonderwoman", 5000, 5.0)
        };

        var checAccounts = new List<Account>
        {
            new CheckingAccount(),
            new CheckingAccount("Larry2"),
            new CheckingAccount("Moe2", 2000),
            new CheckingAccount("Curly2", 5000)
        };

        var trustAccounts = new List<Account>
        {
            new TrustAccount(),
            new TrustAccount("Superman2"),
            new TrustAccount("Batman2", 2000),
            new TrustAccount("Wonderwoman2", 5000, 5.0)
        };

        AccountUtil.Deposit(accounts, 1000);
        AccountUtil.Withdraw(accounts, 2000);

        AccountUtil.Deposit(savAccounts, 1000);
        AccountUtil.Withdraw(savAccounts, 2000);

        AccountUtil.Deposit(checAccounts, 1000);
        AccountUtil.Withdraw(checAccounts, 2000);

        AccountUtil.Deposit(trustAccounts, 1000);
        AccountUtil.Deposit(trustAccounts, 6000);
        AccountUtil.Withdraw(trustAccounts, 2000);
        AccountUtil.Withdraw(trustAccounts, 3000);
        AccountUtil.Withdraw(trustAccounts, 500);
    }
}
