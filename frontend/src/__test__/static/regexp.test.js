import { emailRegExp } from '../../app/helpers/regexp';

describe('email regexp', () => {
  test('asd@asd.com is email format', () => {
    const string = 'asd@asd.com';
    const actual =  string.match(emailRegExp);
    expect(actual).not.toBeNull();
  });

  test('asd@asd.com_ is not email format', () => {
    const string = 'asd@asd.com ';
    const actual =  string.match(emailRegExp);
    expect(actual).toBeNull();
  });

  test('asdasd asdasd is not email format', () => {
    const string = 'sadasd asd asd ';
    const actual =  string.match(emailRegExp);
    expect(actual).toBeNull();
  });

  test('asdasd@asdasd is not email format', () => {
    const string = 'asdasd@asdasd';
    const actual =  string.match(emailRegExp);
    expect(actual).toBeNull();
  });

  test('asdasd@asdasd.com is not email format', () => {
    const string = 'asdasd.com';
    const actual =  string.match(emailRegExp);
    expect(actual).toBeNull();
  });
});
