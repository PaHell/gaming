export enum TimePeriod {
      Day = "d",
      Week = "w",
      OneMonth = "1m",
      SixMonths = "6m",
      Year = "1y",
      Max = "max",
}

export function formatCurrency(value: number, currency: string = '$'): string {
      if (value > 1_000_000_000) {
            return currency + (value / 1_000_000_000) + 'B';
      }
      if (value > 1_000_000) {
            return currency + (value / 1_000_000) + 'M';
      }
      if (value > 1_000) {
            return currency + (value / 1_000) + 'K';
      }
      return currency + value.toString();
}

export function getEnumValues<T>(enumObj: T): T[] {
      return Object.values(enumObj).filter(isNaN) as T[];
}