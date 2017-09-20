// @flow
export const cutWords = (text: string, limit: number): string => {
  const array: string[] = text.split(' ');
  const filteredArray: string[] = array.filter((word, i) => i < limit - 1);
  const newText: string = filteredArray.join(' ');

  if (array.length > limit) return `${newText}...`;
  return newText;
};

export const bla = 1;
