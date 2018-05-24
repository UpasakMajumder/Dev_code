<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TieredPricingSelector.ascx.cs" Inherits="Kadena.CMSFormControls.Kadena.TieredPricingSelector" %>

<table class="j-tiered-pricing-table">
    <tr>
        <th style="padding-right: 5px; padding-bottom: 5px;">
            <cms:LocalizedLabel runat="server" EnableViewState="false" CssClass="control-label editing-form-label" Style="text-align: left;" ResourceString="Kadena.TieredPricingSelector.Items" />
        </th>
        <th style="padding-right: 5px; padding-bottom: 5px;">
            <cms:LocalizedLabel runat="server" EnableViewState="false" CssClass="control-label editing-form-label" Style="text-align: left;" ResourceString="Kadena.TieredPricingSelector.Price" />
        </th>
        <th style="padding-bottom: 5px;">
            <button type="button" class="btn btn-default j-tiered-pricing-new-line-button">
                <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.TieredPricingSelector.NewLine" />
            </button>
        </th>
    </tr>
</table>

<table class="j-tiered-pricing-table-model" style="display: none;">
    <tr>
        <td style="padding-right: 5px; padding-bottom: 5px;">
            <input type="text" maxlength="20" class="form-control j-tiered-pricing-input" data-attr="qty" style="width: 96px;" />
        </td>
        <td style="padding-right: 5px; padding-bottom: 5px;">
            <input type="text" maxlength="20" class="form-control j-tiered-pricing-input" data-attr="price" style="width: 96px;" />
        </td>
        <td style="padding-right: 5px; padding-bottom: 5px;">
            <button type="button" class="btn btn-default j-tiered-pricing-delete-button">
                <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.TieredPricingSelector.Delete" />
            </button>
        </td>
    </tr>
</table>

<input id="inpValue" type="hidden" runat="server" class="j-tiered-pricing-value" />

<script>
    $cmsj(document).ready(function () {
        RestoreTieredPricingData();

        $cmsj(".j-tiered-pricing-new-line-button").click(function (e) {
            e.preventDefault();

            $cmsj(".j-tiered-pricing-table").append($cmsj(".j-tiered-pricing-table-model").find("tr").clone());

            $cmsj(".j-tiered-pricing-delete-button").click(function (e) {
                e.preventDefault();

                $cmsj(this).parent("td").parent("tr").remove();
                SaveTieredPricingData();
            });
            $cmsj(".j-tiered-pricing-input").keyup(function (e) {
                SaveTieredPricingData();
            });
        });
    });

    function SaveTieredPricingData() {
        var result = [];

        $cmsj(".j-tiered-pricing-table").find("tr").each(function (index) {
            // skipping header
            if (index != 0) {
                var item = {
                    quantity: $cmsj(this).find("input[data-attr='qty']").val(),
                    price: $cmsj(this).find("input[data-attr='price']").val()
                };
                result.push(item);
            }
        });
        $cmsj(".j-tiered-pricing-value").val(JSON.stringify(result));
    };

    function RestoreTieredPricingData() {
        if ($cmsj(".j-tiered-pricing-value").val() != "") {
            var data = JSON.parse($cmsj(".j-tiered-pricing-value").val());

            $cmsj.each(data, function (index, value) {
                var item = $cmsj(".j-tiered-pricing-table-model").find("tr").clone();
                $cmsj(item).find("input[data-attr='qty']").val(value.minVal);
                $cmsj(item).find("input[data-attr='price']").val(value.price);

                $cmsj(item).find(".j-tiered-pricing-input").keyup(function (e) {
                    SaveTieredPricingData();
                });

                $cmsj(item).find(".j-tiered-pricing-delete-button").click(function (e) {
                    e.preventDefault();

                    $cmsj(this).parent("td").parent("tr").remove();
                    SaveTieredPricingData();
                });

                $cmsj(".j-tiered-pricing-table").append(item);
            });
        }
    };
</script>
