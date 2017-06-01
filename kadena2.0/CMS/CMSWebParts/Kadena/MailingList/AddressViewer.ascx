<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressViewer.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.AddressViewer" %>

<div class="processed-list__table-block">
    <div class="processed-list__table-heading processed-list__table-heading--error">
        <h3>We have found 8 errors in your file</h3>
        <div class="btn-group btn-group--right">
            <button type="button" class="btn-action btn-action--secondary">Reload .csv</button>
            <button type="button" data-dialog="#mail-list-errors" class="js-dialog btn-action btn-action--secondary">Correct errors</button>
        </div>
    </div>
    <div class="processed-list__table-inner">
        <table class="table processed-list__table--shadow">
            <tbody>
                <tr>
                    <th>Line</th>
                    <th>Name</th>
                    <th>Address Line 1</th>
                    <th>Address Line 2</th>
                    <th>City</th>
                    <th>State</th>
                    <th>Zip Code</th>
                </tr>
                <tr>
                    <td>2</td>
                    <td>Jane Doe</td>
                    <td>2167 225th Ave</td>
                    <td>-</td>
                    <td>San Francisco</td>
                    <td>CA</td>
                    <td>94116</td>
                </tr>
                <tr>
                    <td>42</td>
                    <td>Oliver Ustinovich</td>
                    <td>2167 5th Ave</td>
                    <td>-</td>
                    <td>San Francisco</td>
                    <td>AB</td>
                    <td>94116</td>
                </tr>
                <tr>
                    <td>56</td>
                    <td>Denisa Lorencova</td>
                    <td>2167/273a 25th Ave</td>
                    <td>-</td>
                    <td>Saint Joseph</td>
                    <td>MS</td>
                    <td>94116</td>
                </tr>
                <tr>
                    <td>75</td>
                    <td>John Snow</td>
                    <td>2167/273a 25th Ave</td>
                    <td>-</td>
                    <td>Old York</td>
                    <td>GB</td>
                    <td>94116</td>
                </tr>
            </tbody>
        </table>
        <span class="processed-list__table-helper">To correct your .csv file, view the line number, go to your original file and check the respective record.
          
 

            <svg class="icon help-arrow">
                <use xlink:href="/gfx/svg/sprites/icons.svg#info-arrow" />
            </svg>

        </span>
    </div>
</div>
<div class="processed-list__table-block">
    <div class="processed-list__table-heading processed-list__table-heading--success">
        <h3>332 addresses have been processed successfuly</h3>
        <div class="btn-group btn-group--right">
            <button type="button" class="btn-action btn-action--secondary">Use only correct</button>
        </div>
    </div>
    <div class="processed-list__table-inner">
        <table class="table processed-list__table--shadow">
            <tbody>
                <tr>
                    <th>Name</th>
                    <th>Address Line 1</th>
                    <th>Address Line 2</th>
                    <th>City</th>
                    <th>State</th>
                    <th>Zip Code</th>
                </tr>
                <tr>
                    <td>Jane Doe</td>
                    <td>2167 225th Ave</td>
                    <td>-</td>
                    <td>San Francisco</td>
                    <td>CA</td>
                    <td>94116</td>
                </tr>
                <tr>
                    <td>Oliver Ustinovich</td>
                    <td>2167 5th Ave</td>
                    <td>-</td>
                    <td>San Francisco</td>
                    <td>AB</td>
                    <td>94116</td>
                </tr>
                <tr>
                    <td>Denisa Lorencova</td>
                    <td>2167/273a 25th Ave</td>
                    <td>-</td>
                    <td>Saint Joseph</td>
                    <td>MS</td>
                    <td>94116</td>
                </tr>
                <tr>
                    <td>John Snow</td>
                    <td>2167/273a 25th Ave</td>
                    <td>-</td>
                    <td>Old York</td>
                    <td>GB</td>
                    <td>94116</td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
<div class="btn-group btn-group--left">
    <button type="button" class="btn-action">Save list</button>
</div>
