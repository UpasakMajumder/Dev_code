// @flow
export const consoleException = (text: [] | string, object?: {}) => {
  const message: string = text.toString();

  if (object) {
    console.error(message, object); // eslint-disable-line no-console
  } else {
    console.error(message); // eslint-disable-line no-console
  }
};

export const bla = 1;
