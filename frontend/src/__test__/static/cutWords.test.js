import { cutWords } from '../../app/helpers/string';

describe('cutWords function', () => {
  test('Cut 20 words to 10', () => {
    const string = '1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0';
    const actual =  cutWords(string, 10);
    const expected = '1 2 3 4 5 6 7 8 9...';
    expect(actual).toEqual(expected);
  });

  test('Pass 5 words to 10', () => {
    const string = '1 2 3 4 5';
    const actual =  cutWords(string, 10);
    const expected = '1 2 3 4 5';
    expect(actual).toEqual(expected);
  });
});
