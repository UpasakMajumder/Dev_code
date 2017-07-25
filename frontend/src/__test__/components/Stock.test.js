import React from 'react';
import Shallow from 'react-test-renderer/shallow';
import Stock from '../../app/components/Stock';
import SVG from '../../app/components/SVG';
import expect from 'expect';
import expectJSX from 'expect-jsx';

expect.extend(expectJSX);
describe('Stock somponent', () => {
  test('Stock should be rendered', () => {
    const shallow = Shallow.createRenderer();
    shallow.render(<Stock text={'50 pcs'} type={'out'} />);
    const actual = shallow.getRenderOutput();
    const expected = (
      <div className={'stock stock--out'}>
        <SVG name='stock--out' className='icon-stock icon-stock--out'/>
        50 pcs
      </div>
    );
    expect(actual).toEqualJSX(expected);
  });
});


