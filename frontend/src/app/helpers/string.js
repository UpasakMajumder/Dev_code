export const cutWords = (text, number) => {
  const array = text.split(' ');
  const filteredArray = array.filter((word, i) => i < number - 1);
  const string = filteredArray.join(' ');

  if (array.length > number) return `${string}...`;
  return string;
};

export const bla = 1;
