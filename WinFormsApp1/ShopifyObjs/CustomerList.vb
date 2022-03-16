Imports System.Text.Json.Serialization

Public Class Customer

    <JsonPropertyName("id")>
    Public Property Id As Long

    <JsonPropertyName("email")>
    Public Property Email As String

    <JsonPropertyName("accepts_marketing")>
    Public Property AcceptsMarketing As Boolean

    <JsonPropertyName("created_at")>
    Public Property CreatedAt As DateTime

    <JsonPropertyName("updated_at")>
    Public Property UpdatedAt As DateTime

    <JsonPropertyName("first_name")>
    Public Property FirstName As String

    <JsonPropertyName("last_name")>
    Public Property LastName As String

    <JsonPropertyName("orders_count")>
    Public Property OrdersCount As Integer

    <JsonPropertyName("state")>
    Public Property State As String

    <JsonPropertyName("total_spent")>
    Public Property TotalSpent As String

    <JsonPropertyName("last_order_id")>
    Public Property LastOrderId As Long?

    <JsonPropertyName("note")>
    Public Property Note As String

    <JsonPropertyName("verified_email")>
    Public Property VerifiedEmail As Boolean

    <JsonPropertyName("multipass_identifier")>
    Public Property MultipassIdentifier As String

    <JsonPropertyName("tax_exempt")>
    Public Property TaxExempt As Boolean

    <JsonPropertyName("phone")>
    Public Property Phone As String

    <JsonPropertyName("tags")>
    Public Property Tags As String

    <JsonPropertyName("last_order_name")>
    Public Property LastOrderName As String

    <JsonPropertyName("currency")>
    Public Property Currency As String

    <JsonPropertyName("accepts_marketing_updated_at")>
    Public Property AcceptsMarketingUpdatedAt As DateTime

    <JsonPropertyName("marketing_opt_in_level")>
    Public Property MarketingOptInLevel As String

    <JsonPropertyName("tax_exemptions")>
    Public Property TaxExemptions As Object()

    <JsonPropertyName("admin_graphql_api_id")>
    Public Property AdminGraphqlApiId As String
End Class

Public Class CustomerList

    <JsonPropertyName("customers")>
    Public Property Customers As Customer()
End Class