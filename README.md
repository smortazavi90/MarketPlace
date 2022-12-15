# CodeTest-StoreKey

## Calculate campaign price

The task is to write functionality for calculating campaign price when a customer is doing a
checkout. We have the following campaign types
⁃ Campaigns based on product combination
⁃ Campaigns based on volume, for example 2 for amount or 3 for x amount

### Combo campaigns
For the combo campaign to work, the customer must combine a minimum of two products to get
the campaign price. A combo campaign contains a list of products that the customer can
combine, here is an example:

| EAN | Price |
| --- | ----- |
| 5000112637922, 5000112637939, 7310865004703, 7340005404261, 7310532109090, 7611612222105 | 30 |

Here are some combination examples a customer can make, a product can be combined with
itself.
- 5000112637922 + 7310865004703 = 30
- 7340005404261 + 7310532109090 + 5000112637922 + 7310865004703 = 60

__Note:__ If the customer adds three products to their basket and all of them are in a campaign, the
customer only pays the campaign price for two of them and will pay original price for the third
product

### Volume campaigns
The volume campaigns are based on number of items the customer needs to add to their
basket to trigger the campaign price. For example, you can setup a campaign for Coca-Cola
and the customer needs to buy two to get the campaign price.

| EAN | Price | Minimum purchase quantity |
| --- | ----- | ------------------------- |
| 8711000530085 | 85 | 2 |
| 7310865004703 | 20 | 2 |

__Note:__ If the customer adds a third item to the basket, the customer needs to pay regular price
for the third item and campaign price for the rest
