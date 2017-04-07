export default function separate(str) {
  if (str.length < 4) return str;

  const array = Array.from(str);
  const formattedArray = [];

  array.reverse().forEach((item, index) => {
    if (!(index % 3) && index) formattedArray.push(' ');
    formattedArray.push(item);
  });

  return formattedArray.reverse().join('');
}
