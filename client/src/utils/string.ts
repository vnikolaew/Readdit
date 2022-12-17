export function shorten(s: string, maxLength: number) {
   return s.length > maxLength ? `${s.slice(0, maxLength)}...` : s;
}