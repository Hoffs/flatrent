class FiringLocalStorage {
    private storage: Storage;
    private listeners: { [key: string]: (key: string, value: string, action: "set" | "remove") => void } = {};

    constructor(storage: Storage)
    {
        this.storage = storage;
    }

    public addListener(listener: (key: string, value: string, action: "set" | "remove") => void): string {
        const id = this.getFreeListenerId();
        this.listeners[id] = listener;
        return id;
    }

    public removeListener(key: string): void {
        delete this.listeners[key];
    }

    public setItem(key: string, value: string) {
        this.storage.setItem(key, value);
        this.callListeners(key, value, "set");
    }

    public setItems(...items: Array<{key: string, value: string}>) {
        items.forEach((i) => this.storage.setItem(i.key, i.value));
        this.callListeners("", "", "set");
    }

    public getItem(key: string): string | null {
        return this.storage.getItem(key);
    }

    public removeItem(key: string) {
        this.storage.removeItem(key);
        this.callListeners(key, "", "remove");
    }

    public removeItems(...keys: string[]) {
        keys.forEach((k) => this.storage.removeItem(k));
        this.callListeners("", "", "remove");
    }

    public clear() {
        this.storage.clear();
    }

    private callListeners(key: string, value: string, action: "set" | "remove") {
        Object.keys(this.listeners).forEach((key) => {
            this.listeners[key](key, value, action);
        });
    }

    private getFreeListenerId(): string {
        const usedIds = Object.keys(this.listeners);
        for (let idx = 0; idx < 10000; idx++) {
            if (!usedIds.includes(idx.toString())) {
                return idx.toString();
            }
        }
        return "-1";
    }
}

const fLocalStorage = new FiringLocalStorage(localStorage);

export { fLocalStorage };
