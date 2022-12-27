import { useState, useEffect } from "react";

export function useDebounce<T>(
   value: T,
   delay: number,
   predicate?: (value: T) => boolean
) {
   const [debouncedValue, setDebouncedValue] = useState(value);

   useEffect(() => {
      if (value === "" || !predicate?.(value)) return;

      const handler = setTimeout(() => {
         setDebouncedValue(value);
      }, delay);

      return () => clearTimeout(handler);
   }, [delay, predicate, value]);

   return debouncedValue;
}
