import React from 'react';
import Shallow from 'react-test-renderer/shallow';

export const getShallowComponent = (Component) => {
  const shallow = Shallow.createRenderer();
  shallow.render(Component);
  return shallow.getRenderOutput();
};

export const bla = 1;
