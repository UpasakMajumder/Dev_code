import { createNewUrl } from '../../app/helpers/location';

describe('createNewUrl', () => {
  const loc = {
    host: "localhost:5002",
    hostname: "localhost",
    href: "http://localhost:5002/hi/asdasd/hi?tab=1",
    origin: "http://localhost:5002",
    pathname: "/hi/asdasd/hi",
    port: "5002",
    protocol: "http:",
    search: "?tab=1"
  };

  test('pass nothing', () => {
    const actual = createNewUrl({}, loc);
    const expected = 'http://localhost:5002/hi/asdasd/hi?tab=1';
    expect(actual).toEqual(expected);
  });

  test('pass new protocol https:', () => {
    const actual = createNewUrl({ protocol: 'https:' }, loc);
    const expected = 'https://localhost:5002/hi/asdasd/hi?tab=1';
    expect(actual).toEqual(expected);
  });

  test('pass new host locations:5001', () => {
    const actual = createNewUrl({ host: 'locations:5001' }, loc);
    const expected = 'http://locations:5001/hi/asdasd/hi?tab=1';
    expect(actual).toEqual(expected);
  });

  test('pass new pathname /hi', () => {
    const actual = createNewUrl({ pathname: '/hi' }, loc);
    const expected = 'http://localhost:5002/hi?tab=1';
    expect(actual).toEqual(expected);
  });

  test('pass new search clear', () => {
    const actual = createNewUrl({ search: {
      method: 'clear'
    }}, loc);
    const expected = 'http://localhost:5002/hi/asdasd/hi';
    expect(actual).toEqual(expected);
  });

  test('pass new search with params', () => {
    const actual = createNewUrl({ search: {
      method: 'set',
      props: {
        tab: 1,
        tob: 2,
        hi: '123'
      }
    }}, loc);
    const expected = 'http://localhost:5002/hi/asdasd/hi?tab=1&tob=2&hi=123';
    expect(actual).toEqual(expected);
  });
});
