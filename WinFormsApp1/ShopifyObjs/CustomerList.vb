Imports System.Text.Json.Serialization
Imports Newtonsoft.Json

Public Class Customer

    <JsonProperty("id")>
    <JsonPropertyName("id")>
    Public Property Id As Long

    <JsonProperty("email")>
    <JsonPropertyName("email")>
    Public Property Email As String

    <JsonProperty("accepts_marketing")>
    <JsonPropertyName("accepts_marketing")>
    Public Property AcceptsMarketing As Boolean

    <JsonProperty("created_at")>
    <JsonPropertyName("created_at")>
    Public Property CreatedAt As DateTime

    <JsonProperty("updated_at")>
    <JsonPropertyName("updated_at")>
    Public Property UpdatedAt As DateTime

    <JsonProperty("first_name")>
    <JsonPropertyName("first_name")>
    Public Property FirstName As String

    <JsonProperty("last_name")>
    <JsonPropertyName("last_name")>
    Public Property LastName As String

    <JsonProperty("orders_count")>
    <JsonPropertyName("orders_count")>
    Public Property OrdersCount As Integer

    <JsonProperty("state")>
    <JsonPropertyName("state")>
    Public Property State As String

    <JsonProperty("total_spent")>
    <JsonPropertyName("total_spent")>
    Public Property TotalSpent As String

    <JsonProperty("last_order_id")>
    <JsonPropertyName("last_order_id")>
    Public Property LastOrderId As Long?

    <JsonProperty("note")>
    <JsonPropertyName("note")>
    Public Property Note As String

    <JsonProperty("verified_email")>
    <JsonPropertyName("verified_email")>
    Public Property VerifiedEmail As Boolean

    <JsonProperty("multipass_identifier")>
    <JsonPropertyName("multipass_identifier")>
    Public Property MultipassIdentifier As String

    <JsonProperty("tax_exempt")>
    <JsonPropertyName("tax_exempt")>
    Public Property TaxExempt As Boolean

    <JsonProperty("phone")>
    <JsonPropertyName("phone")>
    Public Property Phone As String

    <JsonProperty("tags")>
    <JsonPropertyName("tags")>
    Public Property Tags As String

    <JsonProperty("last_order_name")>
    <JsonPropertyName("last_order_name")>
    Public Property LastOrderName As String

    <JsonProperty("currency")>
    <JsonPropertyName("currency")>
    Public Property Currency As String

    <JsonProperty("accepts_marketing_updated_at")>
    <JsonPropertyName("accepts_marketing_updated_at")>
    Public Property AcceptsMarketingUpdatedAt As DateTime

    <JsonProperty("marketing_opt_in_level")>
    <JsonPropertyName("marketing_opt_in_level")>
    Public Property MarketingOptInLevel As String

    <JsonProperty("tax_exemptions")>
    <JsonPropertyName("tax_exemptions")>
    Public Property TaxExemptions As Object()

    <JsonProperty("admin_graphql_api_id")>
    <JsonPropertyName("admin_graphql_api_id")>
    Public Property AdminGraphqlApiId As String
End Class

Public Class CustomerList

    <JsonProperty("customers")>
    <JsonPropertyName("customers")>
    Public Property Customers As Customer()
End Class