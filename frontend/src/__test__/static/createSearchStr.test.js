import { createSearchStr } from '../../app/helpers/location';

describe('createSearchStr', () => {
  test('Should create str using obj', () => {
    const obj = {
      hello: 'howareyou',
      cao: 'cacao'
    };
    const actual = createSearchStr(obj);
    const expected = '?hello=howareyou&cao=cacao';
    expect(actual).toEqual(expected);
  });
});
