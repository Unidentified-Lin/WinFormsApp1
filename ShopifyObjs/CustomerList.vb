Imports Newtonsoft.Json

Public Class Customer

    <JsonProperty("id")>
    Public Property Id As Long

    <JsonProperty("email")>
    Public Property Email As String

    <JsonProperty("accepts_marketing")>
    Public Property AcceptsMarketing As Boolean

    <JsonProperty("created_at")>
    Public Property CreatedAt As DateTime

    <JsonProperty("updated_at")>
    Public Property UpdatedAt As DateTime

    <JsonProperty("first_name")>
    Public Property FirstName As String

    <JsonProperty("last_name")>
    Public Property LastName As String

    <JsonProperty("orders_count")>
    Public Property OrdersCount As Integer

    <JsonProperty("state")>
    Public Property State As String

    <JsonProperty("total_spent")>
    Public Property TotalSpent As String

    <JsonProperty("last_order_id")>
    Public Property LastOrderId As String

    <JsonProperty("note")>
    Public Property Note As String

    <JsonProperty("verified_email")>
    Public Property VerifiedEmail As Boolean

    <JsonProperty("multipass_identifier")>
    Public Property MultipassIdentifier As String

    <JsonProperty("tax_exempt")>
    Public Property TaxExempt As Boolean

    <JsonProperty("phone")>
    Public Property Phone As String

    <JsonProperty("tags")>
    Public Property Tags As String

    <JsonProperty("last_order_name")>
    Public Property LastOrderName As String

    <JsonProperty("currency")>
    Public Property Currency As String

    <JsonProperty("accepts_marketing_updated_at")>
    Public Property AcceptsMarketingUpdatedAt As DateTime

    <JsonProperty("marketing_opt_in_level")>
    Public Property MarketingOptInLevel As String

    <JsonProperty("tax_exemptions")>
    Public Property TaxExemptions As Object()

    <JsonProperty("admin_graphql_api_id")>
    Public Property AdminGraphqlApiId As String
End Class

Public Class CustomerList

    <JsonProperty("customers")>
    Public Property Customers As Customer()
End Class