namespace ForegroundAppKiller;

public interface IShortcutStorage
{
    public KeyboardShortcut GetShortcut();
    public void SaveShortcut(KeyboardShortcut shortcut);
}