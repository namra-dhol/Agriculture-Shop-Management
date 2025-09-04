
//#region GetProductsByType & Variants

//[HttpGet("bytype/{productTypeId}")]
//public async Task<ActionResult<IEnumerable<Product>>> GetProductsByTypeIncludingVariants(int productTypeId)
//{
//    var products = await context.Products
//        .Include(p => p.ProductVariants)  
//        .Where(p => p.ProductTypeId == productTypeId)
//        .ToListAsync();

//    if (products == null || products.Count == 0)
//        return NotFound($"No products found for ProductTypeId = {productTypeId}");

//    return Ok(products);

//    //var studentsWithMultipleCourses = students.GroupJoin(courses, s => s.Rno, c => c.Rno,
//    //(s, cs) => (Student: s, CourseCount: cs.Count()))
//    //.Where(sc => sc.CourseCount > 1);
//    //Console.WriteLine(string.Join(Environment.NewLine, studentsWithMultipleCourses
//    //    .Select(sc => $"Rno: {sc.Student.Rno}, Name: {sc.Student.Name}, Course Count: {sc.CourseCount}")));

//    //var products = context.Products.Join(ProductType , )

//}

//#endregion


// #region GetProductsByType & Variants (using select)

// [HttpGet("bytype/{productTypeId}")]
// public async Task<ActionResult> GetProductsByTypeIncludingVariants(int productTypeId)
// {
//     var products = await context.Products
//         .Where(p => p.ProductTypeId == productTypeId)
//         .Select(p => new
//         {
//             p.ProductId,
//             p.ProductName,
//             p.ProductImg,
//             ProductVariants = p.ProductVariants.Select(v => new
//             {
//                 v.Size,
//                 v.Price
//             }).ToList()
//         })
//         .ToListAsync();

//     if (products == null || products.Count == 0)
//         return NotFound($"No products found for ProductTypeId = {productTypeId}");

//     return Ok(products);
// }
// #endregion