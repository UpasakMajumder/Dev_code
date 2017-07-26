// @flow
export default function separate(str: string, symbol: string): string {
  if (str.length < 4) return str;

  const array: string[] = Array.from(str);
  const formattedArray: string[] = [];

  array.reverse().forEach((item: string, index: number) => {
    if (!(index % 3) && index) formattedArray.push(symbol);
    formattedArray.push(item);
  });

  return formattedArray.reverse().join('');
}
