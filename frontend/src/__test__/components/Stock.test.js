import React from 'react';
import expect from 'expect';
import expectJSX from 'expect-jsx';
import { getShallowComponent } from '../_helpers/shallow';
import SVG from '../../app/components/_dump/SVG';
import Stock from '../../app/components/_dump/Stock';

expect.extend(expectJSX);

describe('Stock somponent', () => {
  test('Stock should be rendered', () => {
    const Component = <Stock text={'50 pcs'} type={'out'} />;
    const actual = getShallowComponent(Component);
    const expected = (
      <div className={'stock stock--out'}>
        <SVG name='stock--out' className='icon-stock icon-stock--out'/>
        50 pcs
      </div>
    );
    expect(actual).toEqualJSX(expected);
  });
});


