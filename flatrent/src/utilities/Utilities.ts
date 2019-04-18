export const joinClasses = (...classNames: string[]): string => {
    return classNames.join(" ");
};

export const dayOrDays = (days: number): string => days.toString(10).slice(-1) === "1" ? "diena" : "dienos";
